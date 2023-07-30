<script setup lang="ts">
import { ref } from "vue";
import SourceForm from "@/components/Content/ManageSourcesPage/SourceForm.vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import SourceObject from "@/model/Source";
import { useSourcesStore } from "@/stores/sources";
import { storeToRefs } from "pinia";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";

const sourcesStore = useSourcesStore();
const { errorMessage } = storeToRefs(sourcesStore);

const createSource = async (source: SourceObject) => {
	if (!source) {
		return;
	}

	const createSourceResult = await sourcesStore.addSourceAsync(source);

	if (!createSourceResult) {
		NotificationError("Failed to create source", `Source couldn't be created.`);
		showDrawer();
		return;
	}

	NotificationSuccess(
		"Source Created!",
		`Source ${source.name} created successfully`
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
	<a-button
		type="primary"
		@click="showDrawer"
	>
		<template #icon><PlusOutlined /></template>
		Add source manually
	</a-button>
	<a-drawer
		title="Add new source manually"
		:width="800"
		:visible="visible"
		:body-style="{ paddingBottom: '20px' }"
		:footer-style="{ textAlign: 'right' }"
		@close="onClose"
	>
		<SourceForm
			:source-input="null"
			@on-closed="onClose"
			@on-submitted="createSource"
		/>
	</a-drawer>
</template>
