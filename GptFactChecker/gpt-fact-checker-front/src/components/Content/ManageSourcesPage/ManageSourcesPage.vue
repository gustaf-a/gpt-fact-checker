<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useSourcesStore } from "@/stores/sources";
import { storeToRefs } from "pinia";
import ManageSource from "./ManageSource.vue";
import SourceObject from "@/model/Source";

const sourcesStore = useSourcesStore();
const { sources, loadingSources } = storeToRefs(sourcesStore);

const initLoading = ref(true);

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
	initLoading.value = false;
});
</script>

<template>
	<a-list
		class="demo-loadmore-list"
		:loading="loadingSources || initLoading"
		item-layout="horizontal"
		:data-source="sources"
	>
		<template #renderItem="{ item: source }">
			<a-list-item>
				<template #actions>
					<a key="more">Details</a>
					<a key="edit">Edit</a>
					<a key="delete">Remove</a>
				</template>
				<a-skeleton
					:title="false"
					:loading="!!source.loading"
				>
					<a-list-item-meta :description="source.description">
						<template #title>
                            <div>
                                <p>{{ source.name }}</p>
                            </div>
						</template>
					</a-list-item-meta>
					<div class="list-item-content">
                        <a :href="source.sourceUrl" target="_blank">Source</a>
                        content
                    </div>
				</a-skeleton>
			</a-list-item>
		</template>
	</a-list>

	<div class="manage-sources-list">
		<div v-if="!loadingSources">
			<div
				v-for="source in sources"
				class="source-container"
				:key="source.id"
			>
				<ManageSource :source="source" />
			</div>
		</div>
		<div
			v-else
			class="loading-sources"
		>
			<a-spin size="large" />
		</div>
	</div>
</template>

<style scoped>
.manage-sources-list {
}

.loading-sources {
	display: flex;
	justify-content: center;
	margin-top: 10vh;
}
</style>
