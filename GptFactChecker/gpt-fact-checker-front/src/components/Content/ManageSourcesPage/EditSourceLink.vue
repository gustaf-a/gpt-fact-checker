<script setup lang="ts">
import { ref } from "vue";
import SourceForm from "@/components/Content/ManageSourcesPage/SourceForm.vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import Source from "@/model/Source";
import { useSourcesStore } from "@/stores/sources";
import { storeToRefs } from "pinia";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";

interface Props {
	sourceInput: Source | null;
}

const { sourceInput } = defineProps<Props>();

const sourcesStore = useSourcesStore();
const { errorMessage } = storeToRefs(sourcesStore);

const updateSource = async (source: Source) => {
	if (!source) {
		return;
	}

	const updateSourceResult = await sourcesStore.updateSourceAsync(source);

	if (!updateSourceResult) {
		NotificationError("Failed to update source", `Source couldn't be updated.`);
		showDrawer();
		return;
	}

	NotificationSuccess(
		"Source updated!",
		`Source ${source.name} updated successfully`
	);

	onClose();
};

const visible = ref<boolean>(false);

const showDrawer = () => {
	visible.value = true;
};

const onClose = () => {
	visible.value = false;
};
</script>

<template>
	<a key="edit"
		@click="showDrawer">Edit</a>
	<a-drawer
		title="Edit source"
		:width="800"
		:visible="visible"
		:body-style="{ paddingBottom: '20px' }"
		:footer-style="{ textAlign: 'right' }"
		@close="onClose"
	>
		<SourceForm
			:source-input="sourceInput"
			@on-closed="onClose"
			@on-submitted="updateSource"
		/>
	</a-drawer>
</template>
