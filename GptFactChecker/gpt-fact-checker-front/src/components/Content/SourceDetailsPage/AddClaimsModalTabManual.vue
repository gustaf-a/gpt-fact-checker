<script setup lang="ts">
import Claim from "@/model/Claim";
import type { FormInstance } from "ant-design-vue";
import type { Rule } from "ant-design-vue/es/form";
import { reactive, ref } from "vue";

const emits = defineEmits(["claim"]);

const errorMessage = ref("");

interface FormState {
	claimRawText: string;
	claimSummarized: string;
	tags: string;
	keepValues: boolean;
}

const formState = reactive<FormState>({
	claimRawText: "",
	claimSummarized: "",
	tags: "",
	keepValues: false,
});

const formRef = ref<FormInstance>();

const rules: Record<string, Rule[]> = {
	claimRawText: [{ required: true, message: "Raw text is required." }],
	claimSummarized: [
		{ required: true, message: "Summary of claim is required." },
	],
};

const onValidationFailed = (errorInfo: any) => {
	console.log("Validation failed:", errorInfo);
};

const submitForm = () => {
	let claim: Claim | null = null;

	try {
		claim = getClaim(formState);
	} catch {
		errorMessage.value = "Failed to create claim from form data.";
		console.log("Failed to create claim from form data.");
		return;
	}

	emits("claim", claim);

	clearValues();
};

function getClaim(form: FormState): Claim {
	const claim = new Claim();

	claim.claimRawText = form.claimRawText;
	claim.claimSummarized = form.claimSummarized;

	if (form.tags !== "") {
		claim.tags = form.tags.split(",");
	}

	return claim;
}

function clearValues() {
	if (formState.keepValues) return;

	formState.claimSummarized = "";
	formState.claimRawText = "";
	formState.tags = "";
}
</script>

<template>
	<div>
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
						label="ClaimRawText"
						name="claimRawText"
					>
						<a-input-group compact>
							<a-input
								v-model:value="formState.claimRawText"
								placeholder="Please enter claim raw text"
								required
							/>
						</a-input-group>
					</a-form-item>
				</a-col>
			</a-row>
			<a-row>
				<a-col :span="24">
					<a-form-item
						label="ClaimSummarized"
						name="claimSummarized"
					>
						<a-input-group compact>
							<a-input
								v-model:value="formState.claimSummarized"
								placeholder="Please enter claim summary"
								required
							/>
						</a-input-group>
					</a-form-item>
				</a-col>
			</a-row>
			<a-row>
				<a-col :span="24">
					<a-form-item
						label="Tags separated by comma ,"
						name="tags"
					>
						<a-input v-model:value="formState.tags" />
					</a-form-item>
				</a-col>
			</a-row>
			<a-row>
				<a-col
					:span="16"
					class="keep-values-col"
				>
					<div class="keep-values-div">
						<p class="keep-values-label">Keep Values</p>
						<a-form-item>
							<a-switch v-model:checked="formState.keepValues" />
						</a-form-item>
					</div>
				</a-col>
				<a-col :span="8">
					<div class="form-buttons">
						<a-form-item>
							<a-button
								class="form-button"
								@click="clearValues"
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
</template>

<style scoped>
.keep-values {
	display: flex;
	justify-content: end;
	align-items: center;
}

.keep-values-div {
	display: flex;
	justify-content: end;
}

.keep-values-label {
	margin-top: 0.5vh;
	margin-right: 1vw;
}

.form-buttons {
	display: flex;
	justify-content: end;
}

.form-button {
	margin-right: 1vw;
}
</style>
