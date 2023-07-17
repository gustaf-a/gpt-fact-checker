<script setup lang="ts">
import { onMounted, ref } from "vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import Source from "@/model/Source";
import { useClaimsStore } from "@/stores/claims";
import ClaimObject from "@/model/Claim";
import ClaimCheck from "@/model/ClaimCheck";
import { generateGuid } from "@/utils/guid";
import AddFactChecksAi from "./AddClaimChecksModalTabAiFactCheck.vue";
import type ClaimCheckResult from "@/model/ClaimCheckResult";

interface Props {
	source: Source;
}
const { source } = defineProps<Props>();

const emits = defineEmits(["claimCheckResults"]);

const claimCheckResultsToBeAdded = ref<ClaimCheckResult[]>([]);

const visible = ref<boolean>(false);
const claimsStore = useClaimsStore();
const claims = ref<ClaimObject[]>([]);
const initLoading = ref<boolean>(true);
const loadingClaims = ref<boolean>(true);

const activeKey = ref("1");

const showModal = () => {
	visible.value = true;
};

const receiveClaimCheckResults = (claimCheckResults: ClaimCheckResult[]) => {
	claimCheckResults.forEach((claimCheckResult) => {
		receiveClaimCheckResult(claimCheckResult);
	});
};

const receiveClaimCheckResult = (claimCheckResult: ClaimCheckResult) => {
	if (
		!claimCheckResult.isFactChecked ||
		!claimCheckResult.claim ||
		!claimCheckResult.claimCheck
	) {
		console.log("Invalid claim check result.");
		return;
	}

	if (claimCheckResult.claim && claimCheckResult.claim.id == "") {
		claimCheckResult.claim.id = generateGuid();
	}

	if (!claimCheckIsValid(claimCheckResult.claimCheck)) {
		console.log("Invalid claim check", claimCheckResult.claimCheck);
		return;
	}

	claimCheckResultsToBeAdded.value.push(claimCheckResult);
};

function claimCheckIsValid(claimCheck: ClaimCheck) {
	if (!claimCheck.label || claimCheck.label === "") return false;

	if (!claimCheck.claimCheckText || claimCheck.claimCheckText === "")
		return false;

	return true;
}

const getClaimCheckDescriptionLine = (claimCheck: ClaimCheck): string => {
	const references = getReferencesString(claimCheck);
	return (
		getClaimCheckSummarizedString(claimCheck) +
		(references !== "" ? ` (${references})` : "")
	);
};

const maxReferencesToShow = 2;

const getReferencesString = (claimCheck: ClaimCheck): string => {
	let references = "";
	if (claimCheck.references && claimCheck.references.length > 0) {
		const maxReferences = Math.min(
			claimCheck.references.length,
			maxReferencesToShow
		);
		references = claimCheck.references
			.slice(0, maxReferences)
			.map((reference) => `"${reference}"`)
			.join(", ");
		if (claimCheck.references.length > maxReferences) {
			references += `, +${claimCheck.references.length - maxReferences}`;
		}
	}
	return references;
};

const getClaimCheckSummarizedString = (claimCheck: ClaimCheck): string => {
	return (
		claimCheck.label +
		": " +
		claimCheck.claimCheckText.slice(0, 30).trim() +
		".."
	);
};

const removeClaimCheck = (index: number) => {
	claimCheckResultsToBeAdded.value = claimCheckResultsToBeAdded.value.splice(index, 1);
};

const handleOk = () => {
	emits("claimCheckResults", claimCheckResultsToBeAdded.value);

	claimCheckResultsToBeAdded.value = [];

	visible.value = false;
};

onMounted(async () => {
	await fetchClaims();
});

async function fetchClaims() {
	if (!source) {
		initLoading.value = false;
		return;
	}

	loadingClaims.value = true;

	try {
		const claimsFound = await claimsStore.getClaimsAsync(source.id);

		claims.value = claimsFound;
	} catch (error) {
		console.error(error);
	} finally {
		loadingClaims.value = false;
		initLoading.value = false;
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
			v-model:visible="visible"
			@ok="handleOk"
		>
			<a-tabs v-model:activeKey="activeKey">
				<a-tab-pane
					key="1"
					tab="Fact Check with AI"
					force-render
				>
					<div>
						<AddFactChecksAi
							:claims="claims"
							@claimCheckResults="receiveClaimCheckResults"
						/>
					</div>
				</a-tab-pane>
			</a-tabs>

			<div>
				<h3>ClaimChecks to be added</h3>
				<a-list
					item-layout="horizontal"
					:data-source="claimCheckResultsToBeAdded"
				>
					<template #renderItem="{ item: claimCheckResult }">
						<a-list-item>
							<template #actions>
								<!-- <a key="edit">Edit</a> -->
								<a
									key="delete"
									@click="removeClaimCheck(claimCheckResult.claimCheck.id)"
									>Remove</a
								>
							</template>
							<a-list-item-meta>
								<template #title>
									<div>
										<p class="claimCheck-description">
											{{ getClaimCheckDescriptionLine(claimCheckResult.claimCheck) }}
										</p>
									</div>
								</template>
							</a-list-item-meta>
						</a-list-item>
					</template>
				</a-list>
			</div>
		</a-modal>
	</div>
</template>

<style scoped></style>
