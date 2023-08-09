<script setup lang="ts">
import { reactive, ref, onMounted } from "vue";
import Claim from "@/model/Claim";
import { EditOutlined } from "@ant-design/icons-vue";
import type { FormInstance } from "ant-design-vue";
import type { Rule } from "ant-design-vue/es/form";

interface Props {
	claimInput: Claim;
}
const { claimInput } = defineProps<Props>();

const emits = defineEmits(["claimToUpdate"]);

const visible = ref<boolean>(false);

const showModal = () => {
	visible.value = true;
};

function claimIsValid(claim: Claim) {
	if (!claim.claimSummarized || claim.claimSummarized === "") return false;

	if (!claim.claimRawText || claim.claimRawText === "") return false;

	return true;
}

const handleOk = () => {
	let claim: Claim | null = null;

	try {
		claim = getClaim(formState);
	} catch {
		errorMessage.value = "Failed to create claim from form data.";
		console.log("Failed to create claim from form data.");
		return;
	}
	
	emits("claimToUpdate", claim);

	clearValues();
	
	visible.value = false;
};

// - - - - FORM


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

function setFormState(claimInput: Claim) {
	formState.claimRawText = claimInput.claimRawText || formState.claimRawText;
	formState.claimSummarized = claimInput.claimSummarized || formState.claimSummarized;
	
	if(claimInput.tags)
		formState.tags = claimInput.tags.join(",") || formState.tags;
}

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

function getClaim(form: FormState): Claim {
	const claim = new Claim();

	claim.id = claimInput.id;

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

onMounted(() => {
	setFormState(claimInput);
});
</script>

<template>
	<div>
		<a
			class="claim-management-button"
			@click="showModal"
			><EditOutlined
		/></a>

		<a-modal
			v-model:visible="visible"
			@ok="handleOk"
		>
			<a-tabs>
				<a-tab-pane
					key="1"
					tab="Edit Claim"
				>
					<div>
						<a-form
							:model="formState"
							:rules="rules"
							ref="formRef"
							layout="vertical"
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
						</a-form>
					</div>
				</a-tab-pane>
			</a-tabs>
		</a-modal>
	</div>
</template>

<style scoped></style>
