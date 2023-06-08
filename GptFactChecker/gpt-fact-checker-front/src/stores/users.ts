import { ref } from "vue";
import { defineStore } from "pinia";
import User from "@/model/User";
import UserLogin from "@/model/UserLogin";
import UserSignup from "@/model/UserSignup";
import { supabase } from "@/utils/supabase";
import type { Session } from "@supabase/supabase-js";
import { ErrorMessages } from "@/utils/errors";

const UsersKey = "users";

export const useUserStore = defineStore(UsersKey, () => {
	const user = ref<User | null>();
	const session = ref<Session | null>();

	const errorMessage = ref<string>();

	const loadingUser = ref<boolean>(false);

	const Roles = {
		ADMIN: "admin",
		GUEST: "guest",
		NEWUSER: "newuser",
		ADDCLAIMS: "addclaims",
		EDITCLAIMS: "editclaims",
		DELETECLAIMS: "deleteclaims",
		ADDSOURCES: "addsources",
		EDITSOURCES: "editsources",
		DELETESOURCES: "deletesources",
		ADDCLAIMCHECKS: "addclaimchecks",
		EDITCLAIMCHECKS: "editclaimchecks",
		DELETECLAIMCHECKS: "deleteclaimchecks",
		ADDCLAIMCHECKREACTIONS: "addclaimcheckreactions",
		EDITCLAIMCHECKREACTIONS: "editclaimcheckreactions",
		DELETECLAIMCHECKREACTIONS: "deleteclaimcheckreactions",
	};

	const userHasRole = (role: string): boolean => {
		if (!user) {
			errorMessage.value = "No valid user found. Access denied.";
			return false;
		}

		if (!user.value?.roles) {
			errorMessage.value = "No valid user roles found. Access denied.";
			return false;
		}

		if (user.value.roles.includes(Roles.ADMIN)) return true;

		if (!user.value.roles.includes(role)) {
			errorMessage.value = "User without needed user role. Access denied.";
			return false;
		}

		return true;
	};

	const handleSignup = async (userSignup: UserSignup): Promise<void> => {
		const { email, password, userName, name } = userSignup;

		if (!validateEmail(email)) {
			errorMessage.value = "Email is invalid.";
			return;
		}

		if (!validatePassword(password)) {
			errorMessage.value = "Password cannot be empty";
			return;
		}

		if (!validateUserName(userName)) {
			errorMessage.value = "UserName invalid.";
			return;
		}

		if (!validateName(name)) {
			errorMessage.value = "Please provide a valid name.";
			return;
		}

		errorMessage.value = "";

		try {
			loadingUser.value = true;

			if (!CheckUniqueUserName(userName)) {
				errorMessage.value = "Username already exists.";
				return;
			}

			if (!CheckUniqueEmail(email)) {
				errorMessage.value = "User with email already exists.";
				return;
			}

			const { error: signupError } = await supabase.auth.signUp({
				email,
				password,
			});

			if (signupError) {
				errorMessage.value = "User already registered";
				return;
			}

			const {data, error:addUserDataError} = await supabase.from(UsersKey).insert({
				name,
				username: userName,
				email,
				roles: [Roles.NEWUSER],
			});

			if (addUserDataError) {
				errorMessage.value = "Error encountered when adding user data. Please contact support.";
				return;
			}
		} catch (error) {
			errorMessage.value = `${ErrorMessages.USER_SIGNUP_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
		} finally {
			loadingUser.value = false;
		}
	};

	const loginInAsGuest = () => {
		user.value = {
			id: "guestuser",
			name: "GuestUser",
			userName: "GuestUser",
			email: "guestuser@guestmail.com",
			roles: [Roles.GUEST],
		} as User;
	};

	const handleLogout = async () => {
		await supabase.auth.signOut();

		user.value = null;
		session.value = null;
	};

	const handleLogin = async (userLogin: UserLogin): Promise<void> => {
		const { email, password } = userLogin;

		errorMessage.value = "";

		if (!validateEmail(email)) {
			errorMessage.value = "Email is invalid.";
			return;
		}

		if (!validatePassword(password)) {
			errorMessage.value =
				"Invalid password. Password must be at least 8 characters and contain a mix of upper & lower case, numbers, and special characters";
			return;
		}

		try {
			loadingUser.value = true;

			const { data, error } = await supabase.auth.signInWithPassword({
				email,
				password,
			});

			if (error) {
				user.value = null;
				session.value = null;
				errorMessage.value = "Login failed.";
				return;
			}

			session.value = data.session;

			await getExistingUser();
		} catch {
			user.value = null;
			session.value = null;
			errorMessage.value = "Error encountered when logging in. Login failed.";
		} finally {
			loadingUser.value = false;
		}
	};

	const getExistingUser = async () => {
		try{
			loadingUser.value = true;

			const { data } = await supabase.auth.getUser();

			if (!data.user) {
				return;
			}
	
			const { data: userInfo } = await supabase
				.from(UsersKey)
				.select()
				.eq("email", data.user.email)
				.single();

			if(!userInfo){
				errorMessage.value = "Failed to get user info. Please try logging in again.";
				return;
			}

			const existingUser = {
				// id: userInfo.,
				name: userInfo.name,
				userName: userInfo.username,
				email: userInfo.email,
				roles: userInfo.roles,
				about: userInfo.about,
				profileImage: userInfo.profileimage,
			} as User;

			if(!existingUser){
				errorMessage.value = "Failed to get get UserInfo from object. Please contact support.";
				return;
			}
	
			user.value = existingUser;
		}
		catch(error){
			errorMessage.value = `${ErrorMessages.USER_SIGNUP_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
		}
		finally{
			loadingUser.value = false;
		}
	}

	async function CheckUniqueUserName(userName: string): Promise<boolean> {
		const response = await supabase
			.from(UsersKey)
			.select()
			.eq("username", userName)
			.single();

		return !response || !response.data;
	}

	async function CheckUniqueEmail(email: string): Promise<boolean> {
		const response = await supabase
			.from(UsersKey)
			.select()
			.eq("email", email)
			.single();

		return !response || !response.data;
	}

	// length should be at least 8 characters.
	// should contain at least one uppercase letter.
	// should contain at least one lowercase letter.
	// should contain at least one digit.
	// should contain at least one special character.
	function validatePassword(password: string): boolean {
		return (
			password.length >= 8 &&
			//   && /[A-Z]/.test(password)
			/[a-z]/.test(password)
			//   && /\d/.test(password)
			//   && /[!@#$%^&*()\-=_+[\]{};':"\\|,.<>/?]/.test(password)
		);
	}

	function validateEmail(email: string): boolean {
		return (
			String(email)
				.toLowerCase()
				.match(
					/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
				) != null
		);
	}

	const minUserNameLength = 3;
	const maxUserNameLength = 20;

	function validateUserName(userName: string): boolean {
		const regex = new RegExp(
			`^[a-zA-Z0-9_ÅÄÖåäöÆæØø]{${minUserNameLength},${maxUserNameLength}}$`
		);
		return regex.test(userName);
	}

	const minNameLength = 5;
	const maxNameLength = 20;

	function validateName(name: string): boolean {
		const nameLength = name.length;
		return nameLength >= minNameLength && nameLength <= maxNameLength;
	}

	return {
		user,
		Roles,
		errorMessage,
		loadingUser,
		userHasRole,
		loginInAsGuest,
		handleLogin,
		handleLogout,
		handleSignup,
		getExistingUser
	};
});
