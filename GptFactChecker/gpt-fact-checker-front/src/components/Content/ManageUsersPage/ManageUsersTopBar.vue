<script setup lang="ts">
import AddNewSourceDrawer from "@/components/Content/ManageSourcesPage/AddNewSourceDrawer.vue";
import { useUserStore } from "@/stores/users";
import { ref, watch, onMounted } from "vue";
import { useUsersAdminStore } from "@/stores/usersadmin"
import FilterOptions from "@/model/FilterOptions"
import User from "@/model/User";

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;

const usersAdminStore = useUsersAdminStore();
const { getAllUsers, applyFilters, users } = usersAdminStore;

const allUsers = ref<User[]>([]);

const searchText = ref<string>('');

watch(searchText, () => {
    applyFilters(new FilterOptions(searchText.value.toLowerCase()));
});

const fetchAllUsers = async ()  => {
	await getAllUsers();

	allUsers.value = users;
}

onMounted(() => {
	fetchAllUsers();
});
</script>

<template>
	<div class="manage-source-top-bar">
		<div>
			<a-input-search
				v-model:value.trim="searchText"
				class="nav-item navigation-search"
				placeholder="Search.."
			/>
		</div>
		<div
			class="new-source-container"
			v-if="userHasRole(Roles.ADDSOURCES)"
		>
			<AddNewSourceDrawer />
		</div>
	</div>
</template>

<style scoped>
.manage-source-top-bar {
	margin-top: 2vh;
}
</style>
