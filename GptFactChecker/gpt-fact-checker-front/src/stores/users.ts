import { ref } from "vue";
import { defineStore } from "pinia";
import User from "@/model/User";

const UsersKey = "users";

export const useUserStore = defineStore(UsersKey, () => {
	const user = ref<User>();

	const Roles = {
		ADMIN: "admin",
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
			console.log("Access denied.");
			return false;
		}

		if (!user.value?.roles) {
			console.log("Access denied.");
			return false;
		}

		if (user.value.roles.includes(Roles.ADMIN)) return true;

		if (!user.value.roles.includes(role)) {
			console.log("Access denied.");
			return false;
		}

		return true;
	};

	user.value = {
		id: "aaaa1000",
		name: "Test Testson",
		userName: "test-user-1",
		email: "test1@mail.com",
		roles: [Roles.ADDCLAIMS, Roles.ADDCLAIMCHECKS, Roles.ADDCLAIMCHECKREACTIONS],
	} as User;

	return {
		user,
		Roles,
		userHasRole,
		// errorMessage,
		// loading,
		// loadingUser,
		// handleLogin,
		// handleLogout,
		// handleSignup,
		// getUser,
	};
});
