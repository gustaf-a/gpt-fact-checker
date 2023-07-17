import { ref } from "vue";
import { defineStore } from "pinia";
import User from "@/model/User";
import FilterOptions from "@/model/FilterOptions";
import { supabase } from "@/utils/supabase";
import { ErrorMessages } from "@/utils/errors";
import { filterUsers } from "@/utils/searchfilters";

const UsersKey = "users";

export const useUsersAdminStore = defineStore(UsersKey, () => {
	const users = ref<User[]>([]);
	const filteredSources = ref<User[]>([]);

	function applyFilters(filterOptions: FilterOptions | undefined) {
		filteredSources.value = filterUsers(users.value, filterOptions);
	}

	const loadingUsers = ref<boolean>(false);
	const errorMessage = ref<string>();

	async function getAllUsers() {
		try {
			loadingUsers.value = true;

			const { data: usersFromDatabase } = await supabase
				.from(UsersKey)
				.select(); //will fail because of rowbased security?

			if (!usersFromDatabase) {
				errorMessage.value = "Failed to get users from database.";
				return;
			}

			console.log(usersFromDatabase);

			//TODO
			// users.value = usersFromDatabase();
		} catch (error) {
			errorMessage.value = `${ErrorMessages.USER_SIGNUP_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
		} finally {
			loadingUsers.value = false;
		}
	}

	return {
		users,
		errorMessage,
		loadingUsers,
		getAllUsers,
		applyFilters,
	};
});
