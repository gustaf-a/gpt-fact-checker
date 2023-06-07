<script setup lang="ts">
import { ref, reactive } from "vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import type { Rule } from "ant-design-vue/es/form";
import type { FormInstance } from "ant-design-vue";
import Claim from "@/model/Claim";
import ClaimCheck from "@/model/ClaimCheck";
import { generateGuid } from "@/utils/guid";
import { claimCheckVerdicts } from "@/model/ClaimCheckVerdicts";

const emits = defineEmits(["addClaimCheck"]);

interface Props {
	claim: Claim;
}

interface FormState {
	verdict: string;
	claimCheckText: string;
}

const formState = reactive<FormState>({
	verdict: "Select Verdict",
	claimCheckText: "",
});

const formRef = ref<FormInstance>();

const rules: Record<string, Rule[]> = {
	claimCheckText: [
		{ required: true, message: "Text for fact check is required." },
		{ min: 20, message: 'Text for fact check should be at least 20 characters long.' },
		{ max: 1000, message: 'Text for fact check should not exceed 1000 characters.' },
	],
	verdict: [
		{ required: true, message: "Verdict is required." },
		{ validator: (rule, value) => value !== "Select Verdict" ? Promise.resolve() : Promise.reject('Please change the verdict from default value.') },
	],
};

const { claim } = defineProps<Props>();

const visible = ref<boolean>(false);

const showModal = () => {
	visible.value = true;
};

const onValidationFailed = (errorInfo: any) => {
	console.log("Validation failed:", errorInfo);
};

const onClose = () => {
	visible.value = false;

	resetValues();
};

const handleOk = () => {
	if (!formState) {
		console.log("Form in invalid state.");
		return;
	}
};

const submitForm = () => {
	const claimCheckObject = createClaimCheckObject(formState);
	if (!claimCheckObject) {
		console.log("Failed to convert from form to ClaimCheckObject.", formState);
		return;
	}

	emits("addClaimCheck", claimCheckObject);

	resetValues();

	visible.value = false;
};

function resetValues() {
	formRef.value?.resetFields();
}

function createClaimCheckObject(formState: FormState): ClaimCheck | null {
	try {
		const claimCheckObject = new ClaimCheck();

		claimCheckObject.id = generateGuid();

		claimCheckObject.claimCheckText = formState.claimCheckText;
		claimCheckObject.label = formState.verdict;

		return claimCheckObject;
	} catch (error) {
		console.log("Failed to convert form to source object.", error);
		return null;
	}
}
</script>

<template>
	<div>
		<a-button
			shape="circle"
			@click="showModal"
		>
			<template #icon><PlusOutlined /></template>
		</a-button>

		<a-modal
			class="add-claim-check-modal"
			v-model:visible="visible"
			@close="onClose"
			@ok="handleOk"
			width="80vw"
			footer=""
		>
			<div class="claim-presentation">
				<p class="claim-summary">Claim: {{ claim.claimSummarized }}</p>
				<p class="claim-raw">"{{ claim.claimRawText }}"</p>
			</div>
			<div class="claim-chech-input">
				<a-form
					:model="formState"
					:rules="rules"
					ref="formRef"
					layout="vertical"
					@finish="submitForm"
					@finishFailed="onValidationFailed"
				>
					<a-row>
						<a-col :span="24">
							<a-form-item
								label="Verdict"
								name="verdict"
							>
								<a-select
									v-model:value="formState.verdict"
									placeholder="Please choose source type"
									required
								>
									<a-select-option
										v-for="verdict in claimCheckVerdicts"
										:key="verdict.name"
										:value="verdict.name"
										>{{ verdict.name }}</a-select-option
									>
								</a-select>
							</a-form-item>
						</a-col>
					</a-row>
					<a-row>
						<a-col :span="24">
							<a-form-item
								label="Fact Check Text"
								name="claimCheckText"
							>
								<a-textarea
									v-model:value="formState.claimCheckText"
									:rows="5"
									placeholder="Please be respectful, concise and provide sources when possible."
									required
								/>
							</a-form-item>
						</a-col>
					</a-row>
					<a-row>
						<a-col :span="24">
							<div class="form-buttons">
								<a-form-item>
									<a-button
										class="form-button"
										@click="resetValues"
										>Clear</a-button
									>
								</a-form-item>
								<a-form-item>
									<a-button
										type="primary"
										html-type="submit"
										>Submit</a-button
									>
								</a-form-item>
							</div>
						</a-col>
					</a-row>
				</a-form>
			</div>
		</a-modal>
	</div>
</template>

<style scoped>
.add-claim-check-modal {
	width: 80vw;
}

.claim-summary {
	font-weight: 600;
	font-size: larger;
}

.claim-raw {
	font-style: italic;
}

.form-buttons {
	display: flex;
	justify-content: end;
}

.form-button {
	margin-right: 1vw;
}
</style>
