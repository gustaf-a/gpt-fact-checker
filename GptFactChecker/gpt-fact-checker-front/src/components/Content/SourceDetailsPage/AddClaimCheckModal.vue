<script setup lang="ts">
import { ref, reactive } from "vue";
import { storeToRefs } from "pinia";
import { PlusOutlined } from "@ant-design/icons-vue";
import type { Rule } from "ant-design-vue/es/form";
import type { FormInstance } from "ant-design-vue";
import Claim from "@/model/Claim";
import ClaimCheck from "@/model/ClaimCheck";
import { generateGuid } from "@/utils/guid";
import { claimCheckVerdicts } from "@/model/ClaimCheckVerdicts";
import { useFactChecksStore } from "@/stores/factCheckings";
import { useUserStore } from "@/stores/users";

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;

const factCheckStore = useFactChecksStore();
const { errorMessages, loading } = storeToRefs(factCheckStore);

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
		{
			min: 20,
			message: "Text for fact check should be at least 20 characters long.",
		},
		{
			max: 1000,
			message: "Text for fact check should not exceed 1000 characters.",
		},
	],
	verdict: [
		{ required: true, message: "Verdict is required." },
		{
			validator: (rule, value) =>
				value !== "Select Verdict"
					? Promise.resolve()
					: Promise.reject("Please change the verdict from default value."),
		},
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

// --- AI creation

async function createAiFactCheckForClaim() {
	//if already creating, then nothing.
	if (!claim || !claim.id) return;

	if (loading.value) return;

	const claimCheckResult = await factCheckStore.factCheckClaimAsync(claim.id);

	if (!claimCheckResult) {
		errorMessages.value.push("Failed to fact check claim.");
		return;
	}

	emits("addClaimCheck", claimCheckResult.claimCheck);

	resetValues();

	visible.value = false;
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
					<div v-if="loading">
						<a-spin></a-spin>
					</div>
					<div
						class="manual-input"
						v-else
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
				</div>
					<a-row>
						<a-col :span="24">
							<div class="form-buttons">
								<a-form-item>
									<a-button
										:disabled="loading"
										class="form-button"
										@click="resetValues"
										>Clear</a-button
									>
								</a-form-item>
								<a-form-item>
									<a-button
										:disabled="loading"
										type="primary"
										html-type="submit"
										>Submit</a-button
									>
								</a-form-item>
							</div>
						</a-col>
					</a-row>
					<a-row>
						<a-col :span="24">
							<div
								class="button-row"
								v-if="userHasRole(Roles.ADDCLAIMCHECKSWITHAI)"
							>
								<h3>or create with AI</h3>
								<a-button
									shape="circle"
									@click="createAiFactCheckForClaim"
								>
									<template #icon><PlusOutlined /></template>
								</a-button>
							</div>
							<div v-for="msg in errorMessages">
								<p class="error-message-text">{{ msg }}</p>
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

.button-row {
	display: flex;
	align-items: center;
	justify-content: center;
}

.form-buttons {
	display: flex;
	justify-content: end;
}

.form-button {
	margin-right: 1vw;
}
</style>
