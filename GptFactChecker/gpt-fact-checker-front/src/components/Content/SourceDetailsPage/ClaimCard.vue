<script setup lang="ts">
import ClaimObject from "@/model/Claim";
import ClaimCheckList from "./ClaimCheckList.vue";

interface Props {
	claim: ClaimObject;
}

const { claim } = defineProps<Props>();

//TODO base on claimcheck verdict
const cardColor = () => {
	if (!claim.claimChecks) return "gray";

	switch (claim.claimChecks[0].label) {
		case "Correct":
			return "green";
		case "Incorrect":
			return "red";
		case "Not checked":
		default:
			return "gray";
	}
};

</script>

<template>
	<a-card class="claim-card">
		<a-row class="claim-card-content">
			<a-col
				:span="10"
				class="claim-card-left"
			>
				<h3 class="claim-summary">{{ claim.claimSummarized }}</h3>
				<p class="claim-raw-text">"{{ claim.claimRawText }}"</p>
			</a-col>
			<a-col
				:span="14"
				class="claim-card-right"
			>
				<ClaimCheckList :claim="claim" />
			</a-col>
		</a-row>
	</a-card>
</template>

<style scoped>
.claim-card {
	width: 90vw;
	min-height: 25vh;
	box-shadow: 0 2px 8px #f0f0f0;
}

.claim-card-content {
	margin: 0;
	padding: 0;
	display: flex;
}

.claim-card-left {
	display: flex;
	flex-direction: column;
	align-content: center;
	justify-content: center;
	border-right: 1px solid #f0f0f0;
}

.claim-summary {
	margin: 0 1vw;
	font-size: 1em;
}

.claim-raw-text {
	margin: 1vw 1vw 0 1vw;
	font-size: 0.9em;
	color: rgba(0, 0, 0, 0.9);
	font-weight: 200;
	font-style: italic;
	text-align: center;
}

.claim-card-right {
	padding: 0 1vw;
	margin-top: 0;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: start;
}
</style>
