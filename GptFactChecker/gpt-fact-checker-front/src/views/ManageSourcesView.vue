<script setup lang="ts">
import Container from "@/components/Container.vue";
import ManageSourcesPage from "@/components/Content/ManageSourcesPage/ManageSourcesPage.vue";
import ManageSourcesTopBar from "@/components/Content/ManageSourcesPage/ManageSourcesTopBar.vue";
import { useUserStore } from "@/stores/users";
import NotFound from "@/components/NotFound.vue"

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;
</script>

<template>
	<div class="content-container"
	v-if="userHasRole(Roles.EDITSOURCES)">
		<Container>
			<ManageSourcesTopBar />
			<a-divider />
			<div class="manage-sources-container">
				<ManageSourcesPage />
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
