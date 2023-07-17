<script setup lang="ts">
import { storeToRefs } from "pinia";
import Claim from "@/model/Claim";
import {computed } from 'vue';
import { useFactChecksStore } from "@/stores/factCheckings";

interface Props {
	claims: Claim[];
}
const { claims } = defineProps<Props>();

const factCheckStore = useFactChecksStore();
const { errorMessages, loading } = storeToRefs(factCheckStore);

const emits = defineEmits(["claimCheckResults"]);

const claimsWithoutFactChecks = computed(() =>
  claims.filter((claim) => claim.claimChecks == null || claim.claimChecks.length == 0)
);

const claimsWithFactChecks = computed(() =>
  claims.filter((claim) => claim.claimChecks != null && claim.claimChecks.length > 0)
);

const factCheckAllClaims = async () => {
	if (!claims || claims.length == 0) return;

	var claimIds = claims.filter(claim => claim.id !== undefined).map(claim => claim.id as string);

	await factCheckClaims(claimIds);
};

const factCheckUncheckedClaims = async () => {
	if (!claimsWithoutFactChecks.value || claimsWithoutFactChecks.value.length == 0) return;

	var claimIds = claimsWithoutFactChecks.value.filter(claim => claim.id !== undefined).map(claim => claim.id as string);

	await factCheckClaims(claimIds);
};

const factCheckClaims = async (claimIdsToCheck: string[]) => {
	if (!claimIdsToCheck || claimIdsToCheck.length == 0) return;

	if (loading.value) return;

	const claimCheckResults = await factCheckStore.factCheckClaimsAsync(claimIdsToCheck);

	if(!claimCheckResults || claimCheckResults.length == 0){
		errorMessages.value.push("No fact check results received.")
		return;
	}

	emits("claimCheckResults", claimCheckResults);
};


</script>

<template>
	<div>
		<p class="about-text">
			Use AI to fact check claims.
		</p>
		<p>Claims without fact checks: {{ claimsWithoutFactChecks.length }}</p>
    	<p>Claims with existing fact checks: {{ claimsWithFactChecks.length }}</p>
	</div>

	<div v-if="loading">
		<a-spin></a-spin>
	</div>
	<div v-else>
		<hr />
		<div v-for="msg in errorMessages">
			<p class="error-message-text">{{ msg }}</p>
		</div>
	</div>

	<div class="form-buttons">
		<a-button 
			class="fact-check-button"
			type="primary"
			:disabled="loading"
			:loading="loading"
			@click="factCheckUncheckedClaims"
			>Fact Check Unchecked</a-button
		>
		<a-button
			type="primary"
			:disabled="loading"
			:loading="loading"
			@click="factCheckAllClaims"
			>Fact Check All</a-button
		>
	</div>
</template>

<style scoped>
.form-buttons {
	display: flex;
	align-items: center;
	justify-content: center;
	margin: 1vh;
}

.fact-check-button{
	margin: 0 2vw;
}

.error-message-text {
	color: red;
}
</style>