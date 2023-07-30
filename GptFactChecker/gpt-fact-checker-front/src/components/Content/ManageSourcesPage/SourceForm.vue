<script setup lang="ts">
import { reactive, ref, onMounted } from "vue";
import Source from "@/model/Source";
import type { Rule } from "ant-design-vue/es/form";
import type { FormInstance } from "ant-design-vue";
import { useSourcesStore } from "@/stores/sources";
import { storeToRefs } from "pinia";
import { generateGuid } from "@/utils/guid";

interface Props {
	sourceInput: Source | null;
}

const { sourceInput } = defineProps<Props>();

const emits = defineEmits(["onClosed", "onSubmitted"]);

const isEditMode = ref<boolean>(false);

const sourcesStore = useSourcesStore();
const { errorMessage } = storeToRefs(sourcesStore);

onMounted(() => {
	if (sourceInput != null) {
		setFormState(sourceInput);
		isEditMode.value = true;
	}
});


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

function setFormState(sourceInput: Source) {
	formState.name = sourceInput.name || formState.name;
	formState.language = sourceInput.language || formState.language;
	formState.description = sourceInput.description || formState.description;
	formState.tags = sourceInput.tags.join(",") || formState.tags;
	formState.sourceType = sourceInput.sourceType || formState.sourceType;
	formState.sourcePerson = sourceInput.sourcePerson || formState.sourcePerson;
	formState.sourceContext =
		sourceInput.sourceContext || formState.sourceContext;
	formState.sourceUrl = sourceInput.sourceUrl || formState.sourceUrl;
	formState.sourceRawText =
		sourceInput.sourceRawText || formState.sourceRawText;
	formState.coverImageUrl =
		sourceInput.coverImageUrl || formState.coverImageUrl;
	formState.sourceCreatedDate =
		sourceInput.sourceCreatedDate || formState.sourceCreatedDate;
}

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

const onClose = () => {
	emits("onClosed");
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

	emits("onSubmitted", sourceObject);

	formRef.value?.resetFields();
};

function getSourceObject(form: FormState): Source | null {
	try {
		let source = sourceInput;

		if (source == null) {
			source = new Source();
		}

		if(source.id == null || source.id == "")
		{
			source.id = generateGuid();
		}

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
</script>

<template>
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
</template>
