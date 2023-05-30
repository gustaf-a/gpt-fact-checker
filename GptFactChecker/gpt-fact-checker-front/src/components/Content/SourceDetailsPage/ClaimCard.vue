<script setup lang="ts">
import ClaimObject from "@/model/Claim";
import FactCheckList from "./FactCheckList.vue";

interface Props {
	claim: ClaimObject;
}

const { claim } = defineProps<Props>();

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
	<div class="claim-card">
		<a-card>
			<a-row>
				<a-col>
					<h3 class="claim-title">{{ claim.claimSummarized }}</h3>
				</a-col>
				<a-col>
					<div class="horizontal-divider"></div>
					<div class="claim-card-content">
						<q class="claim-raw-text">{{ claim.claimRawText }}</q>
					</div>
					<div class="horizontal-divider"></div>
				</a-col>
			</a-row>
			<div class="fact-checks">
				<!-- <FactCheckList :claim="claim" /> -->
			</div>
		</a-card>
	</div>
</template>

<style scoped>
.claim-card {
	width: 90vw;
	border: 1px solid #f0f0f0;
	border-radius: 2px;
	box-shadow: 0 2px 8px #f0f0f0;
}

.claim-title {
	margin: 0;
	font-size:medium
}

.horizontal-divider {
	height: 1px;
	background-color: #f0f0f0;
	margin: 0.5vh 0;
}

.vertical-divider {
  width: 1px;
  height: 100%;
  background-color: #f0f0f0;
}

.claim-card-content {
	margin: 0;
	display: flex;
}

.claim-raw-text {
	font-size: 1em;
	color: rgba(0, 0, 0, 0.9);
	margin-top: 0;
	margin-bottom: 0px;
	font-weight: 200;
	font-style: italic;
}
</style>
