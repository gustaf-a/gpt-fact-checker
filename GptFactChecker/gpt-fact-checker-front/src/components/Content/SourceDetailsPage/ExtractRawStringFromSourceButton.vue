<script setup lang="ts">
import { ref } from "vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import Source from "@/model/Source";
import { useSourcesStore } from "@/stores/sources";
import { useSourceExtractionsStore } from "@/stores/sourceExtractions";
import { storeToRefs } from "pinia";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";

interface Props {
	source: Source;
}
const { source } = defineProps<Props>();

const sourcesStore = useSourcesStore();
const sourceExtractionsStore = useSourceExtractionsStore();
const { errorMessage: sourcesErrorMessage } = storeToRefs(sourcesStore);

const { errorMessages: sourceExtractionErrorMessages, loadingExtractedSource } =
	storeToRefs(sourceExtractionsStore);

const errorMessages = ref<string[]>([]);

const updateSource = async (source: Source) => {
	if (!source) {
		return;
	}

	const updateSourceResult = await sourcesStore.updateSourceAsync(source);

	if (!updateSourceResult) {
		NotificationError("Failed to update source", `Source couldn't be updated.`);
		errorMessages.value.push(sourcesErrorMessage.value);
		return;
	}

	NotificationSuccess(
		"Source updated!",
		`Source ${source.name} updated successfully`
	);
};

const extractRawText = async () => {
	errorMessages.value = [];

	if (loadingExtractedSource.value) return;

	if (source == null || source.id == null) {
		errorMessages.value.push("Invalid source");
		return;
	}

	let sourceWithRawText: Source | undefined = undefined;

	try {
		sourceWithRawText = await sourceExtractionsStore.extractSourceAsync(
			source.id
		);
	} catch {
		errorMessages.value.push("Failed to extract raw text from source.");
	}

	sourceExtractionErrorMessages.value.forEach((msg) => {
		errorMessages.value.push(msg);
	});

	if (sourceWithRawText == undefined) {
		errorMessages.value.push("Failed to receive valid source object.");
		NotificationError(
			"Failed to extract raw text from source",
			`Source couldn't be analyzed.`
		);
		return;
	}

	if (errorMessages.value.length > 0) return;

	NotificationSuccess(
		"Raw text extracted!",
		`Source ${source.name} analyzed successfully. Saving results..`
	);

	await updateSource(sourceWithRawText);
};
</script>

<template>
	<a-button
		type="primary"
		@click="extractRawText"
		:loading="loadingExtractedSource"
	>
		<template #icon><PlusOutlined /></template>
		Extract Raw Text Automatically
	</a-button>
	<p
		v-for="msg in errorMessages"
		class="error-text"
	>
		{{ msg }}
	</p>
	<div
		class="spinner-container"
		v-if="loadingExtractedSource"
	>
		<p class="spinner-text">Analyzing source</p>
		<a-spin
			class="spinner"
			size="large"
		/>
	</div>
</template>

<style scoped>
.modal-title {
	display: flex;
	align-items: center;
	justify-content: center;
}

.spinner-container {
	display: flex;
	align-items: center;
	justify-content: center;
}

.spinner {
	margin-left: 10vw;
}

.error-text {
	color: red;
}
</style>
