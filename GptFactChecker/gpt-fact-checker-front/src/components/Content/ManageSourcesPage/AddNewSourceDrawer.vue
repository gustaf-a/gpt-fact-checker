<script setup lang="ts">
import { reactive, ref } from "vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import SourceObject from "@/model/Source";
import type { Rule } from "ant-design-vue/es/form";
import type { FormInstance } from "ant-design-vue";
import { useSourcesStore } from "@/stores/sources";
import { storeToRefs } from "pinia";
import { generateGuid } from "@/utils/guid";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";

const sourcesStore = useSourcesStore();
const { errorMessage } = storeToRefs(sourcesStore);

// ------------- Form --------------
//TODO
const languages = ["English", "Swedish"];
//TODO
const sourceTypes = ["Text", "Video", "Social Media", "Podcast"];

interface FormState {
	name: string;
	language: string;
	description: string;
	tags: string;
	sourceType: string;
	sourcePerson: string;
	sourceContext: string;
	sourceUrl: string;
	sourceRawText: string;
	sourceCreatedDate: string;
	coverImageUrl: string;
}

const formState = reactive<FormState>({
	name: "",
	language: "English",
	description: "",
	tags: "",
	sourceType: "",
	sourcePerson: "",
	sourceContext: "",
	sourceUrl: "",
	sourceRawText: "",
	coverImageUrl: "",
	sourceCreatedDate: "",
});

const formRef = ref<FormInstance>();

const rules: Record<string, Rule[]> = {
	name: [{ required: true, message: "Name is required." }],
	language: [{ required: true, message: "Language is required." }],
	description: [{ required: true, message: "Description is required." }],
	sourceType: [{ required: true, message: "Source Type is required." }],
	sourcePerson: [{ required: true, message: "Source Person is required." }],
	sourceContext: [{ required: true, message: "Source Context is required." }],
	sourceUrl: [{ required: true, message: "Source URL is required." }],
	sourceCreatedDate: [
		{
			required: true,
			message: "Date is required like this: 1999-12-31",
			pattern: /^\d{4}-\d{2}-\d{2}$/,
		},
	],
};

const onValidationFailed = (errorInfo: any) => {
	console.log("Validation failed:", errorInfo);
};

const submitForm = async () => {
	if (!formState) {
		console.log("Form in invalid state.");
		return;
	}

	const sourceObject = getSourceObject(formState);
	if (!sourceObject) {
		console.log("Failed to convert from form to source object.", formState);
		return;
	}

	visible.value = false;

	const createSourceResult = await sourcesStore.addSourceAsync(sourceObject);

	if (!createSourceResult) {
		NotificationError("Failed to create source", `Source couldn't be created.`);
		visible.value = true;
		return;
	}

	NotificationSuccess(
		"Source Created!",
		`Source ${formState.name} created successfully`
	);

	formRef.value?.resetFields();
};

function getSourceObject(form: FormState): SourceObject | null {
	try {
		const source = new SourceObject();
		source.id = generateGuid();

		source.name = form.name;
		source.language = form.language;
		source.description = form.description;
		source.coverImageUrl = form.coverImageUrl;
		source.tags = form.tags.split(",");
		source.sourceType = form.sourceType;
		source.sourcePerson = form.sourcePerson;
		source.sourceContext = form.sourceContext;
		source.sourceUrl = form.sourceUrl;
		source.sourceRawText = form.sourceRawText;
		source.sourceCreatedDate = form.sourceCreatedDate;
		return source;
	} catch (error) {
		console.log("Failed to convert form to source object.", error);
		return null;
	}
}

//------------- Visual -------------

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
		Add source
	</a-button>
	<a-drawer
		title="Add new source"
		:width="800"
		:visible="visible"
		:body-style="{ paddingBottom: '20px' }"
		:footer-style="{ textAlign: 'right' }"
		@close="onClose"
	>
		<a-form
			:model="formState"
			:rules="rules"
			ref="formRef"
			layout="vertical"
			@finish="submitForm"
			@finishFailed="onValidationFailed"
		>
			<a-row :gutter="16">
				<a-col :span="12">
					<a-form-item
						label="Name"
						name="name"
					>
						<a-input-group compact>
							<a-input
								v-model:value="formState.name"
								placeholder="Please enter source name or title"
								required
							/>
						</a-input-group>
					</a-form-item>
				</a-col>
				<a-col :span="12">
					<a-form-item
						label="Language"
						name="language"
					>
						<a-select
							v-model:value="formState.language"
							placeholder="Please choose source language"
							required
						>
							<a-select-option value="english">English</a-select-option>
							<a-select-option value="swedish">Swedish</a-select-option>
						</a-select>
					</a-form-item>
				</a-col>
			</a-row>
			<a-row :gutter="16">
				<a-col :span="12">
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
				</a-col>
				<a-col :span="12">
					<a-form-item
						label="Source Type"
						name="sourceType"
					>
						<a-select
							v-model:value="formState.sourceType"
							placeholder="Please choose source type"
							required
						>
							<a-select-option
								v-for="sourceTypeOption in sourceTypes"
								:key="sourceTypeOption"
								:value="sourceTypeOption"
								>{{ sourceTypeOption }}</a-select-option
							>
						</a-select>
					</a-form-item>
				</a-col>
			</a-row>
			<a-row :gutter="16">
				<a-col :span="12">
					<a-form-item
						label="Description"
						name="description"
					>
						<a-textarea
							v-model:value="formState.description"
							:rows="3"
							placeholder="Please write a short factful description of this article, video or text."
						/>
					</a-form-item>
				</a-col>
				<a-col :span="12">
					<a-form-item
						label="Raw text"
						name="sourceRawText"
					>
						<a-textarea
							v-model:value="formState.sourceRawText"
							:rows="3"
							placeholder="Raw article text or transcription"
						/>
					</a-form-item>
				</a-col>
			</a-row>
			<a-row :gutter="16">
				<a-col :span="12">
					<a-form-item
						label="Source Author(s)"
						name="sourcePerson"
					>
						<a-input
							v-model:value="formState.sourcePerson"
							required
						/>
					</a-form-item>
				</a-col>
				<a-col :span="12">
					<a-form-item
						label="Context (Company or similar)"
						name="sourceContext"
					>
						<a-input
							v-model:value="formState.sourceContext"
							required
						/>
					</a-form-item>
				</a-col>
			</a-row>
			<a-row :gutter="16">
				<a-col :span="12">
					<a-form-item
						label="Cover Image URL"
						name="coverImageUrl"
					>
						<a-input v-model:value="formState.coverImageUrl" />
					</a-form-item>
				</a-col>
				<a-col :span="12">
					<a-form-item
						label="Date source created or last updated: YYYY-MM-DD"
						name="sourceCreatedDate"
					>
						<a-input
							v-model:value="formState.sourceCreatedDate"
							required
						/>
					</a-form-item>
				</a-col>
			</a-row>
			<a-row :gutter="16">
				<a-col :span="12">
					<a-form-item
						label="Tags separated by comma ,"
						name="tags"
					>
						<a-input v-model:value="formState.tags" />
					</a-form-item>
				</a-col>
			</a-row>
			<a-row :gutter="16">
				<a-col :span="18"> </a-col>
				<a-col :span="3">
					<a-form-item>
						<a-button @click="onClose">Cancel</a-button>
					</a-form-item>
				</a-col>
				<a-col :span="3">
					<a-form-item>
						<a-button
							type="primary"
							html-type="submit"
							>Submit</a-button
						>
					</a-form-item>
				</a-col>
			</a-row>
		</a-form>
	</a-drawer>
</template>
