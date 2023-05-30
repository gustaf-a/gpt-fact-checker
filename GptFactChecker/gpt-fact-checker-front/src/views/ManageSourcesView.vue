<script setup lang="ts">
import Container from "@/components/Container.vue";
import ManageSourcesPage from "@/components/Content/ManageSourcesPage/ManageSourcesPage.vue";
import AddNewSourceDrawer from "@/components/Content/ManageSourcesPage/AddNewSourceDrawer.vue";
import { ref, onMounted } from "vue";
import { useSourcesStore } from "@/stores/sources";
import { storeToRefs } from "pinia";

const sourcesStore = useSourcesStore();
const { sources, loadingSources } = storeToRefs(sourcesStore);

const fetchSources = async () => {
	try {
		await sourcesStore.getSourcesAsync();
	} catch (error) {
		console.error(error);
	} finally {
	}
};

onMounted(() => {
	fetchSources();
});
</script>

<template>
	<div class="content-container">
		<Container>
			<div class="manage-source-top-bar">
				<div>

					<a-input-search
					class="nav-item navigation-search"
					placeholder="Search.."
					/>
				</div>
				<div class="new-source-container">
					<AddNewSourceDrawer />
				</div>
			</div>
			<a-divider />
			<div class="manage-sources-container">
				<ManageSourcesPage/>
			</div>
		</Container>
	</div>
</template>

<style scoped>
.manage-source-top-bar{
	display: flex;
	align-items: center;
	justify-content: space-between;
	margin-top: 2vh;
}

.new-source-container {
	display: flex;
	justify-content: end;
}
</style>
