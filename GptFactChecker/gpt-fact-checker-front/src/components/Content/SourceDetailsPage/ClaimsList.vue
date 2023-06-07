<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useClaimsStore } from "@/stores/claims";
import ClaimObject from "@/model/Claim";
import SourceObject from "@/model/Source";
import ClaimCard from "./ClaimCard.vue";
import AddClaimsModal from "./AddClaimsModal.vue";
import { useUserStore } from "@/stores/users";
import { storeToRefs } from "pinia";

const userStore = useUserStore();
const { user } = storeToRefs(userStore);
const { userHasRole, Roles } = userStore;

interface Props {
	source: SourceObject;
}

const { source } = defineProps<Props>();

const claimsStore = useClaimsStore();
const claims = ref<ClaimObject[]>([]);
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

async function addClaims(claimsToAdd: ClaimObject[]) {
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

async function removeClaim(claimId: string) {}
</script>

<template>
	<div class="claims-header">
		<h3 class="claim-title">Claims</h3>
		<AddClaimsModal
			@claims="addClaims"
			v-if="userHasRole(Roles.ADDCLAIMS)"
		/>
	</div>

	<div class="claims-container">
		<div
			v-for="claim in claims"
			:key="claim.id"
			class="claim"
		>
			<ClaimCard :claim="claim" />
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

.claims-header {
	display: flex;
	align-items: center;
	justify-content: start;
}

.claim-title {
	font-size: 2em;
	padding-top: 5px;
	margin-right: 1vw;
	margin-bottom: 0;
}
</style>
