<script setup lang="ts">
import { onMounted, ref } from "vue";

import { useClaimsStore } from "@/stores/claims";
import ClaimObject from "@/model/Claim";

import SourceObject from "@/model/Source";

import ClaimCard from "@/components/Content/ClaimCard.vue";

interface Props {
	source: SourceObject;
}

const { source } = defineProps<Props>();

console.log(source);

const claimsStore = useClaimsStore();

const loadingClaims = ref<boolean>(true);
const claims = ref<ClaimObject[]>([]);

const fetchClaims = async () => {
	if (!source) return;

	loadingClaims.value = true;

	try {
		const claimsFound = await claimsStore.getClaimsAsync(source.id);
		console.log(claimsFound);
		claims.value = claimsFound;
	} catch (error) {
		console.error(error);
	} finally {
		loadingClaims.value = false;
	}
};

onMounted(async () => {
	await fetchClaims();
});
</script>

<template>
	<div class="claims-container">
		<div
			v-for="claim in claims"
			:key="claim.id"
			class="claim"
		>
			<ClaimCard
				:claim="claim"
				:claim-checks="claim.claimChecks"
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
	cursor: pointer;
}
</style>
