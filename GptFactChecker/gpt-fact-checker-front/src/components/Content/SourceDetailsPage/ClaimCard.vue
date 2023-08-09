<script setup lang="ts">
import ClaimObject from "@/model/Claim";
import ClaimCheckList from "./ClaimCheckList.vue";
import { DeleteFilled } from "@ant-design/icons-vue";
import EditClaimModal from "./EditClaimModal.vue";
import { useUserStore } from "@/stores/users";

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;

interface Props {
	claim: ClaimObject;
}

const emits = defineEmits(["removeClaim", "updateClaim"]);

const { claim } = defineProps<Props>();

//TODO add tags

//make smaller

//

const removeClaim = () => {
	emits("removeClaim", claim.id);
};

const updateClaim = (claimToUpdate: ClaimObject) => {
	emits("updateClaim", claimToUpdate);
};
</script>

<template>
	<a-card class="claim-card">
		<a-row class="claim-card-content">
			<a-col
				:span="10"
				class="claim-card-left"
			>
				<a-row class="claim-content">
					<h3 class="claim-summary">{{ claim.claimSummarized }}</h3>
				</a-row>
				<a-row class="claim-content">
					<p class="claim-raw-text">"{{ claim.claimRawText }}"</p>
				</a-row>
				<a-row class="row-claim-management">
					<div v-if="userHasRole(Roles.EDITCLAIMS)">
						<EditClaimModal
							:claim-input="claim"
							class="claim-management-button"
							@claim-to-update="updateClaim"
						/>
					</div>
					<div v-if="userHasRole(Roles.DELETECLAIMS)">
						<a
							class="claim-management-button"
							@click="removeClaim"
							><DeleteFilled
						/></a>
					</div>
				</a-row>
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
	min-height: 5vh;
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

.claim-content {
}

.claim-summary {
	margin: 0 0.5vw;
	font-size: 1.4em;
}

.claim-raw-text {
	margin: 0.2vw 0.5vw 0 0.5vw;
	font-size: 1em;
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

.row-claim-management {
	display: flex;
	justify-content: end;
	margin-right: 2vw;
}

.claim-management-button {
	margin-right: 1.5vw;
}
</style>
