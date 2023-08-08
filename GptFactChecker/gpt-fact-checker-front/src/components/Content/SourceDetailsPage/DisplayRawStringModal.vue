<script setup lang="ts">
import { ref } from "vue";
import Source from "@/model/Source";
import ExtractRawStringFromSourceButton from "./ExtractRawStringFromSourceButton.vue";

interface Props {
	source: Source;
}
const { source } = defineProps<Props>();

const visible = ref<boolean>(false);

const showModal = () => {
	visible.value = true;
};

const handleOk = () => {
	visible.value = false;
};
</script>

<template>
	<div>
		<a @click="showModal">Show Raw Text</a>

		<a-modal
			v-model:visible="visible"
			@ok="handleOk"
		>
			<h3>{{ source.name }}</h3>
			<div
				v-if="source.sourceRawText"
				class="text-container"
			>
				<p class>
					{{ source.sourceRawText }}
				</p>
			</div>
			<div
				v-else
				class="button-container"
			>
				<ExtractRawStringFromSourceButton :source="source" />
			</div>
		</a-modal>
	</div>
</template>

<style scoped></style>
