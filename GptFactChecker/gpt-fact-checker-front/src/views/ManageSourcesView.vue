<script setup lang="ts">
import Container from "@/components/Container.vue";
import ManageSourcesPage from "@/components/Content/ManageSourcesPage/ManageSourcesPage.vue";
import ManageSourcesTopBar from "@/components/Content/ManageSourcesPage/ManageSourcesTopBar.vue";
</script>

<template>
	<div class="content-container">
		<Container>
			<ManageSourcesTopBar />
			<a-divider />
			<div class="manage-sources-container">
				<ManageSourcesPage />
			</div>
		</Container>
	</div>
</template>

<style scoped>

</style>
