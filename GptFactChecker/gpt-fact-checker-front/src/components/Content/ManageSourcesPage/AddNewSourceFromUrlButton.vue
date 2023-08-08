<script setup lang="ts">
import type { FormInstance } from "ant-design-vue";
import type { Rule } from "ant-design-vue/es/form";
import { reactive, ref } from "vue";
import SourceForm from "@/components/Content/ManageSourcesPage/SourceForm.vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import Source from "@/model/Source";
import { useSourcesStore } from "@/stores/sources";
import { useSourceExtractionsStore } from "@/stores/sourceExtractions";
import { storeToRefs } from "pinia";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";

const sourcesStore = useSourcesStore();
const sourceExtractionsStore = useSourceExtractionsStore();
const { draftSource, errorMessage: sourcesErrorMessage } = storeToRefs(sourcesStore);
const { errorMessages: sourceExtractionErrorMessages, loadingSourceMetaData } =
	storeToRefs(sourceExtractionsStore);

const errorMessages = ref<string[]>([]);

const createSource = async (source: Source) => {
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

// Modal

interface FormState {
	sourceUrl: string;
}

const formState = reactive<FormState>({
	sourceUrl: "",
});

const formRef = ref<FormInstance>();

const rules: Record<string, Rule[]> = {
	sourceUrl: [{ required: true, message: "Source URL is required." }],
};

const onValidationFailed = (errorInfo: any) => {
	console.log("Validation failed:", errorInfo);
};

const visibleModal = ref<boolean>(false);

const showModal = () => {
	visibleModal.value = true;
};

const isValidURL = (url: string): boolean => {
	try {
		new URL(url);
		return true;
	} catch (_) {
		return false;
	}
};

const submitModal = async () => {
	errorMessages.value = [];

	if(loadingSourceMetaData.value)
		return;

	const valid = await formRef.value?.validate();
	if (!valid) {
		return;
	}

	const sourceUrl = formState.sourceUrl;

	if (!isValidURL(sourceUrl)) {
		errorMessages.value.push("Invalid URL");
		return;
	}

	let sourceWithMetaData: Source | undefined = undefined;

	try {
		sourceWithMetaData =
			await sourceExtractionsStore.extractSourceUrlMetaDataAsync(sourceUrl);
	} catch {
		errorMessages.value.push("Failed to extract meta data from url.");
	}

	sourceExtractionErrorMessages.value.forEach((msg) => {
		errorMessages.value.push(msg);
	});

	if (sourceWithMetaData == undefined)
		errorMessages.value.push("Failed to receive valid source object.");

	if (errorMessages.value.length > 0) return;

	draftSource.value = sourceWithMetaData || null;

	showDrawer();
	visibleModal.value = false;
};

// Drawer

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
		@click="showModal"
	>
		<template #icon><PlusOutlined /></template>
		Add Source
	</a-button>

	<a-modal
		v-model:visible="visibleModal"
		:ok-button-props="{ disabled: loadingSourceMetaData }"
		:cancel-button-props="{ disabled: loadingSourceMetaData }"
		@ok="submitModal"
	>
		<div>
			<a-form
				:model="formState"
				:rules="rules"
				ref="formRef"
				layout="vertical"
				@finish="submitModal"
				@finishFailed="onValidationFailed"
			>
				<h3 class="modal-title">Create Source</h3>

				<div class="spinner-container" v-if="loadingSourceMetaData">
					<p class="spinner-text">Loading source data</p>
					<a-spin class="spinner" size="large" />
				</div>
				<div v-else class="">
					<a-row>
						<a-col :span="24">
							<a-form-item
								label="Source URL"
								name="sourceUrl"
							>
								<a-input-group compact>
									<a-input
										v-model:value="formState.sourceUrl"
										placeholder="Please enter source URL"
										required
									/>
								</a-input-group>
							</a-form-item>
							<p
								v-for="msg in errorMessages"
								class="error-text"
							>
								{{ msg }}
							</p>
						</a-col>
					</a-row>
				</div>
			</a-form>
		</div>
	</a-modal>

	<a-drawer
		title="Add Source From URL"
		:width="800"
		:visible="visible"
		:body-style="{ paddingBottom: '20px' }"
		:footer-style="{ textAlign: 'right' }"
		@close="onClose"
	>
		<SourceForm
			@on-closed="onClose"
			@on-submitted="createSource"
		/>
	</a-drawer>
</template>

<style scoped>
.modal-title {
	display: flex;
	align-items: center;
	justify-content: center;
}

.spinner-container{
	display: flex;
	align-items: center;
	justify-content: center;
}

.spinner{
	margin-left: 10vw;
}

.error-text {
	color: red;
}
</style>
