<script setup lang="ts">
import { onMounted, ref } from "vue";

import { useRoute } from "vue-router";
import { useSourcesStore } from "@/stores/sources";

import SourceObject from "@/model/Source";

import ClaimsList from "./ClaimsList.vue";

const route = useRoute();

const loadingSource = ref<boolean>(true);
const sourceId: string = Array.isArray(route.params.sourceId)
	? route.params.sourceId[0].toString()
	: route.params.sourceId.toString();

const sourcesStore = useSourcesStore();
const source = ref<SourceObject | null>(null);

const fetchSource = async () => {
	loadingSource.value = true;

	try {
		const sourceFromStore: SourceObject | null =
			await sourcesStore.getSourceByIdAsync(sourceId);

		if (!sourceFromStore) {
			console.log("Failed to get source with provided ID.");
			return;
		}

		source.value = sourceFromStore;
	} catch (error) {
		console.error(error);
	} finally {
		loadingSource.value = false;
	}
};

onMounted(async () => {
	await fetchSource();
});
</script>

<template>
	<div
		class="source"
		v-if="source"
	>
		<h1>{{ source.name }}</h1>
		<p>
			{{ source.description ? source.description : "No description available" }}
		</p>
		<a-descriptions title="About">
			<a-descriptions-item
				label="URL"
				span="3"
				>{{ source.sourceUrl }}</a-descriptions-item
			>
			<a-descriptions-item label="Type">{{
				source.sourceType
			}}</a-descriptions-item>
			<a-descriptions-item label="Category">{{
				source.tags
			}}</a-descriptions-item>
		</a-descriptions>

		<h3>Claims</h3>
		<ClaimsList :source="source" />
	</div>
	<div v-else>
		<h2>Source not found</h2>
	</div>
</template>

<style scoped>
.source {
	margin-top: 4vh;
}
</style>
