<script setup lang="ts">
import { onMounted, ref } from "vue";
import { storeToRefs } from "pinia";
import SourceObject from "@/model/Source";
import Claim from "@/model/Claim";
import ClaimCheckResult from "@/model/ClaimCheckResult";
import ClaimCard from "./ClaimCard.vue";
import AddClaimsModal from "./AddClaimsModal.vue";
import AddClaimChecksModal from "./AddClaimChecksModal.vue";
import { useUserStore } from "@/stores/users";
import { useClaimsStore } from "@/stores/claims";
import { useClaimCheckStore } from "@/stores/claimChecks";

const userStore = useUserStore();
const { user } = storeToRefs(userStore);
const { userHasRole, Roles } = userStore;

interface Props {
	source: SourceObject;
}

const { source } = defineProps<Props>();

const claimsStore = useClaimsStore();
const claimChecksStore = useClaimCheckStore();

const claims = ref<Claim[]>([]);
const loadingClaims = ref<boolean>(true);
const initLoading = ref<boolean>(true);

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

async function addClaims(claimsToAdd: Claim[]) {
	try {
		const result = await claimsStore.addClaimsAsync(claimsToAdd, source.id);
		if (!result) {
			console.log("Failed to add claims to store.", claimsToAdd);
			return;
		}

		claimsToAdd.forEach((claim) => {
			claims.value.push(claim);
		});
	} catch (error) {
		console.error(error);
	} finally {
		loadingClaims.value = false;
	}
}

async function removeClaim(claimId: string) {
	try {
		const result = await claimsStore.deleteClaimsAsync(claimId);
		if (!result) {
			console.log("Failed to delete claims from store.");
			return;
		}

		claims.value = claims.value.filter((claim) => claim.id !== claimId);
	} catch (error) {
		console.error(error);
	} finally {
		loadingClaims.value = false;
	}
}

async function updateClaim(claim: Claim) {
	if(claim == null) return;
	
	try {
		const result = await claimsStore.updateClaimAsync(claim);
		if (!result) {
			console.log("Failed to delete claims from store.");
			return;
		}

		await fetchClaims();
	} catch (error) {
		console.error(error);
	} finally {
		loadingClaims.value = false;
	}
}

async function addClaimCheckResults(claimCheckResults: ClaimCheckResult[]) {
	try {
		if(claimCheckResults == undefined || claimCheckResults.length == 0)
			return;
		
		const success = await claimChecksStore.addClaimCheckResultsAsync(claimCheckResults)

		if(success) 
			await fetchClaims();

	} catch (error) {
		console.error(error);
	} finally {
		loadingClaims.value = false;
	}
}
</script>

<template>
	<div class="claims-header">
		<a-row class="claims-header-row">
			<a-col
				:span="10"
				class="claims-header-col"
			>
				<h3 class="col-header-title">Claims</h3>
				<AddClaimsModal
					@claims="addClaims"
					:source="source"
					v-if="userHasRole(Roles.ADDCLAIMS)"
				/>
			</a-col>
			<a-col
				:span="14"
				class="claims-header-col"
			>
				<h3 class="col-header-title">Fact Checks</h3>
				<AddClaimChecksModal
					@claimCheckResults="addClaimCheckResults"
					:source="source"
					v-if="userHasRole(Roles.ADDCLAIMCHECKSWITHAI)"
				/>
			</a-col>
		</a-row>
	</div>

	<div class="claims-container">
		<div
			v-for="claim in claims"
			:key="claim.id"
			class="claim"
		>
			<ClaimCard
				:claim="claim"
				@removeClaim="removeClaim"
				@update-claim="updateClaim"
			/>
		</div>
	</div>
</template>

<style scoped>
.claims-container {
	display: flex;
	flex-wrap: wrap;
	padding: 0 1vw;
	align-items: center;
	justify-content: center;
}

.claim {
	margin-right: 1vw;
	margin-top: 1vh;
}

.claims-header-row {
	display: flex;
	align-items: center;
	justify-content: center;
}

.claims-header-col {
	display: flex;
	align-items: center;
	justify-content: center;
}

.col-header-title {
	font-size: 2em;
	padding-top: 5px;
	margin-right: 1vw;
	margin-bottom: 0;
}
</style>
@/model/ClaimCheckResult