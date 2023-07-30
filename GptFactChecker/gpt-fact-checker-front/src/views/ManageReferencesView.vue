<script setup lang="ts">
import Container from "@/components/Container.vue";
import { useUserStore } from "@/stores/users";
import NotFound from "@/components/NotFound.vue"

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;
</script>

<template>
	<div class="content-container"
	v-if="userHasRole(Roles.EDITREFERENCES)">
		<Container>
			<!-- <ManageSourcesTopBar /> -->
			<a-divider />
			<div class="manage-references-container">
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
