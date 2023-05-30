<script setup lang="ts">
import type {
	RuleObject,
	ValidateErrorEntity,
} from "ant-design-vue/es/form/interface";
import { reactive, ref } from "vue";
import type { UnwrapRef } from "vue";
import type Claim from "@/model/Claim";

const emits = defineEmits(["claims"]);

interface FormState {
	serializedJson: string;
}

const formRef = ref();

const formState: UnwrapRef<FormState> = reactive({
	serializedJson: "",
});

const loadedJsonObject = ref<Claim[] | null>(null);

let validateJson = async (rule: RuleObject, serializedValue: string) => {
	if (!serializedValue) {
		return Promise.reject("Please input a serialized JSON object");
	}

	try {
		loadedJsonObject.value = deserialize(serializedValue);
	} catch (error) {
		console.log(error);
	}

	if (!loadedJsonObject.value) {
		return Promise.reject("Failed to deserialize JSON. Ensure camel case variable names.");
	}

	return Promise.resolve();
};

function deserialize(json: string): Claim[] | null {
	const obj = JSON.parse(json);

	if (isClaimsArray(obj)) {
		return obj as Claim[];
	} else {
		return null;
	}
}

function isClaimsArray(obj: any): obj is Claim[] {
	return obj && typeof obj[0].claimRawText === "string";
}

const rules = {
	serializedJson: [
		{ required: true, validator: validateJson, trigger: "submit" },
	],
};

const handleFinish = (values: FormState) => {
	if (!loadedJsonObject.value) {
		return;
	}

	console.log(loadedJsonObject.value);

	emits("claims", loadedJsonObject.value);
};

const handleFinishFailed = (errors: ValidateErrorEntity<FormState>) => {
	console.log(errors);
};

const resetForm = () => {
	formRef.value.resetFields();
};
</script>

<template>
	<a-form
		ref="formRef"
		:model="formState"
		:rules="rules"
		@finish="handleFinish"
		@finishFailed="handleFinishFailed"
	>
		<a-tooltip>
			<template #title>
				JSON array of: { ClaimRawText: string; ClaimSummarized: string; Tags:
				string[]; }
			</template>
			<a-form-item
				has-feedback
				label="Raw JSON"
				name="serializedJson"
			>
				<a-textarea
					v-model:value="formState.serializedJson"
					autocomplete="off"
					:rows="4"
					placeholder="[{...}]"
				/>
			</a-form-item>
		</a-tooltip>

		<div class="form-buttons">
			<a-form-item :wrapper-col="{ span: 12}">
				<a-button
					type="primary"
					html-type="submit"
					>Import claims</a-button
				>
			</a-form-item>
		</div>
	</a-form>
</template>

<style scoped>
.form-buttons {
	display: flex;
	justify-content: end;
}
</style>
