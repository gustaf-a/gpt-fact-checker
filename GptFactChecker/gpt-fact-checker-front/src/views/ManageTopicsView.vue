<script setup lang="ts">
import Container from "@/components/Container.vue";
import { useUserStore } from "@/stores/users";
import NotFound from "@/components/NotFound.vue"

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;
</script>

<template>
	<div class="content-container"
	v-if="userHasRole(Roles.EDITTOPICS)">
		<Container>
			topics
			<!-- <ManageSourcesTopBar /> -->
			<a-divider />
			<div class="manage-topics-container">
				<!-- <ManageSourcesPage /> -->
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
