<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { useSourcesStore } from "@/stores/sources";
import SourceObject from "@/model/Source";
import ClaimsList from "./ClaimsList.vue";

const isLoggedIn = ref(true);
const hasEditingRights = ref(true);

const colors = ["green", "pink", "blue", "orange", "cyan", "red", "purple"];

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
		const sourceFromStore = await sourcesStore.getSourceByIdAsync(sourceId);

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
		<div class="source-heading">
			<h1>{{ source.name }}</h1>
			<a-tag
				v-for="(tag, index) in source.tags"
				:key="index"
				:color="colors[index]"
				>{{ tag }}</a-tag
			>
		</div>
		<p>
			{{ source.description ? source.description : "No description available" }}
		</p>
		<a-descriptions class="description-title" size="small">
			<a-descriptions-item label="Author" class="description-item">{{
				source.sourcePerson
			}}</a-descriptions-item>
				<a-descriptions-item label="Context" class="description-item">{{
					source.sourceContext
				}}</a-descriptions-item>
			<a-descriptions-item label="Language" class="description-item">{{
				source.language
			}}</a-descriptions-item>
			<a-descriptions-item label="Media type" class="description-item">{{
				source.sourceType
			}}</a-descriptions-item>
			<a-descriptions-item label="Source created" class="description-item">{{
				source.sourceCreatedDate
			}}</a-descriptions-item>
			<a-descriptions-item class="description-item">
				<a
				:href="source.sourceUrl"
				target="_blank"
			>
				Source Link</a
			>
			</a-descriptions-item>
		</a-descriptions>

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
