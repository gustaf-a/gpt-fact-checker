<script setup lang="ts">
import Container from "@/components/Container.vue";
import { useUserStore } from "@/stores/users";
import NotFound from "@/components/NotFound.vue"
import ManageUsersPage from "@/components/Content/ManageUsersPage/ManageUsersPage.vue";
import ManageUsersTopBar from "@/components/Content/ManageUsersPage/ManageUsersTopBar.vue";

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;
</script>

<template>
	<div class="content-container"
	v-if="userHasRole(Roles.ADMIN)">
		<Container>
			<ManageUsersTopBar />
			<a-divider />
			<div class="">
				<ManageUsersPage />
			</div>
		</Container>
	</div>
	<div v-else
	>
	<NotFound />
	</div>
</template>

<style scoped>

</style>
