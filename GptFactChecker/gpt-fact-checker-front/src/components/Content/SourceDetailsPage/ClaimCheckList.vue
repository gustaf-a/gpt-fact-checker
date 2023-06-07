<script setup lang="ts">
import { ref, onMounted } from "vue";
import ClaimCheckComment from "./ClaimCheckComment.vue";
import AddClaimCheckModal from "./AddClaimCheckModal.vue";
import ClaimCheckObject from "@/model/ClaimCheck";
import ClaimObject from "@/model/Claim";
import { useClaimCheckStore } from "@/stores/claimChecks";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";
import { generateGuid } from "@/utils/guid";
import { useUserStore } from "@/stores/users";
import { storeToRefs } from "pinia";

const userStore = useUserStore();
const { user } = storeToRefs(userStore);
const { userHasRole, Roles } = userStore;

const claimCheckStore = useClaimCheckStore();
const { loadingClaimChecks } = storeToRefs(claimCheckStore);

interface Props {
	claim: ClaimObject;
}

const { claim } = defineProps<Props>();

const claimChecks = ref<ClaimCheckObject[]>([]);
const initLoading = ref<boolean>(true);

const fetchClaimChecks = async () => {
	if (!claim.id || claim.id === "") return false;

	try {
		const result = await claimCheckStore.getClaimChecksAsync(claim.id);

		claimChecks.value = result;
	} catch (error) {
		console.error(error);
	} finally {
	}
};

onMounted(async () => {
	if (!claim.claimChecks || claim.claimChecks.length === 0) {
		await fetchClaimChecks();
	}

	initLoading.value = false;
});

const addClaimCheck = async (claimCheck: ClaimCheckObject) => {
	if (!claimCheck.id || claimCheck.id === "") {
		claimCheck.id = generateGuid();
	}

	if (!user.value) {
		console.log("Failed to find current user.");
		return false;
	}

	claimCheck.userId = user.value.id;

	const createClaimCheckResult = await claimCheckStore.addClaimCheckAsync(
		claimCheck,
		claim.id
	);

	if (!createClaimCheckResult) {
		NotificationError(
			"Failed to create claimCheck",
			`ClaimCheck couldn't be created: Internal error.`
		);
		return;
	}

	NotificationSuccess("ClaimCheck Created!", `ClaimCheck created successfully`);
};
</script>

<template>
	<div class="fact-check-heading">
		<h3 class="fact-check-title">Fact Checks</h3>
		<AddClaimCheckModal
			@addClaimCheck="addClaimCheck"
			:claim="claim"
			v-if="userHasRole(Roles.ADDCLAIMCHECKS)"
		/>
	</div>
	<div class="horizontal-divider"></div>
	<div class="fact-checks">
		<div v-if="initLoading || loadingClaimChecks">
			<template>
				<a-skeleton active />
			</template>
		</div>
		<div v-else
			v-for="claimCheck in claim.claimChecks"
			:key="claimCheck.id"
			class="claim-check-thread"
		>
			<ClaimCheckComment
				:claimCheck="claimCheck"
				:depth="0"
			/>
		</div>
	</div>
</template>

<style scoped>
.fact-check-heading {
	display: flex;
	align-items: center;
}

.fact-check-title {
	margin: 0 1vw 0 0;
	font-size: 1.2em;
}

.horizontal-divider {
	height: 1px;
	background-color: #f0f0f0;
	margin: 0.5vh 0;
}
</style>
