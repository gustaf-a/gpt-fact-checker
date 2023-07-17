<script setup lang="ts">
import { ref } from "vue";
import { storeToRefs } from "pinia";
import Source from "@/model/Source";
import { useFactExtractionsStore } from "@/stores/factExtractions";

interface Props {
	source: Source;
}
const { source } = defineProps<Props>();

const factExtractionsStore = useFactExtractionsStore();
const { errorMessages, loadingExtractedClaims } =
	storeToRefs(factExtractionsStore);

const sourceRawTextExists = ref<boolean>(
	source.sourceRawText != undefined && source.sourceRawText.trim() != ""
);

const emits = defineEmits(["claims"]);

const extractClaims = async () => {
	if (!source) return;

	if (loadingExtractedClaims.value) {
		return;
	}

	const extractedClaims =
		await factExtractionsStore.extractClaimsFromSourceWithAIAsync(source.id);

	emits("claims", extractedClaims);
};
</script>

<template>
	<div>
		<div v-if="sourceRawTextExists">
			<p class="about-text">
				Use AI to extract claims made in the raw text registered for the source.
			</p>

			<div v-if="loadingExtractedClaims">
				<a-spin></a-spin>
			</div>
			<div v-else>
				<hr />
				<div v-for="msg in errorMessages">
					<p class="error-message-text">{{ msg }}</p>
				</div>
			</div>

			<div class="form-buttons">
				<a-button
					type="primary"
					:disabled="loadingExtractedClaims"
					:loading="loadingExtractedClaims"
					@click="extractClaims"
					>Extract Claims</a-button
				>
			</div>
		</div>
		<div v-else>
			<p class="about-text">
				Source raw text missing. AI claims extraction not possible.
			</p>
		</div>
	</div>
</template>

<style scoped>
.form-buttons {
	display: flex;
	justify-content: end;
}

.error-message-text {
	color: red;
}
</style>
