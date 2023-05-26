<script setup lang="ts">
import ClaimObject from "@/model/Claim";

interface Props {
	claim: ClaimObject;
}

const { claim } = defineProps<Props>();

console.log(claim);

const cardColor = () => {
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
	<div class="claim-card">
		<a-card
			:bordered="true"
			:style="{ borderColor: cardColor }"
		>
			<h3 class="claim-text">{{ claim.claimSummarized }}</h3>
            <p>{{ claim.claimChecks[0].label }}</p>
			<p>{{ claim.claimChecks[0].claimCheckText }}</p>
		</a-card>
	</div>
</template>

<style scoped>
.claim-card {
	width: 35vw;
	border: 1px solid #f0f0f0;
	border-radius: 2px;
	box-shadow: 0 2px 8px #f0f0f0;
}
</style>
