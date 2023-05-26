<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useSourcesStore } from "@/stores/sources";
import SourceObject from "@/model/Source";
import SourceCard from "./SourceCard.vue";
import Container from "@/components/Container.vue";
import NotFound from "@/components/404NotFound.vue";

const sourcesStore = useSourcesStore();
const sources = ref<SourceObject[]>([]);

const loadingSources = ref<boolean>(true);

const fetchSources = async () => {
	loadingSources.value = true;

	try {
		await sourcesStore.getSourcesAsync();
		sources.value = sourcesStore.sources;
	} catch (error) {
		console.error(error);
	} finally {
		loadingSources.value = false;
	}
};

onMounted(() => {
	fetchSources();
});
</script>

<template>
	<Container>
		<div v-if="sources.length">
			<h2 class="sources-headline">Sources checked</h2>
			<a-divider class="divider" />
			<div class="sources-container">
				<div
					v-for="source in sources"
					:key="source.id"
					class="source"
				>
					<SourceCard :source="source" />
				</div>
			</div>
		</div>
		<div v-else>
			<NotFound />
		</div>
	</Container>
</template>

<style scoped>
.sources-headline {
	margin-top: 5vh;
	opacity: 0.7;
	margin-bottom: 0;
}

.divider {
	margin-top: 0;
	margin-bottom: 0;
}

.sources-container {
	display: flex;
	flex-wrap: wrap;
	align-items: center;
	justify-content: center;
}

.source {
	cursor: pointer;
}
</style>
