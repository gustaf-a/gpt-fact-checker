<script setup lang="ts">
import { ref } from "vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import Claim from "@/model/Claim";
import Source from "@/model/Source";
import { generateGuid } from "@/utils/guid";
import AddClaimsFromJson from "./AddClaimsModalTabFromJson.vue";
import AddClaimsManual from "./AddClaimsModalTabManual.vue";
import AddClaimsAiExtraction from "./AddClaimsModalTabAiExtraction.vue";

interface Props {
	source: Source;
}
const { source } = defineProps<Props>();

const emits = defineEmits(["claims"]);

const claimsToBeAdded = ref<Claim[]>([]);

const visible = ref<boolean>(false);

const activeKey = ref("1");

const showModal = () => {
	visible.value = true;
};

const receiveClaims = (claims: Claim[]) => {
	claims.forEach((claim) => {
		receiveClaim(claim);
	});
};

const receiveClaim = (claim: Claim) => {
	if (!claim.id || claim.id === "") {
		claim.id = generateGuid();
	}

	if (!claimIsValid(claim)) {
		console.log("Invalid claim", claim);
		return;
	}

	claimsToBeAdded.value.push(claim);
};

function claimIsValid(claim: Claim) {
	if (!claim.claimSummarized || claim.claimSummarized === "") return false;

	if (!claim.claimRawText || claim.claimRawText === "") return false;

	return true;
}

const maxTagsToShow = 2;

const getTagsString = (claim: Claim): string => {
	let tags = "";
	if (claim.tags && claim.tags.length > 0) {
		const maxTags = Math.min(claim.tags.length, maxTagsToShow);
		tags = claim.tags
			.slice(0, maxTags)
			.map((tag) => `"${tag}"`)
			.join(", ");
		if (claim.tags.length > maxTags) {
			tags += `, +${claim.tags.length - maxTags}`;
		}
	}
	return tags;
};

const getClaimDescriptionLine = (claim: Claim): string => {
	const tags = getTagsString(claim);
	return getClaimSummarizedString(claim) + (tags !== "" ? ` (${tags})` : "");
};

const getClaimSummarizedString = (claim: Claim): string => {
	return claim.claimSummarized.slice(0, 30).trim() + "..";
};


const removeClaim = (index: number) => {
	claimsToBeAdded.value = claimsToBeAdded.value.splice(index, 1);
};

const handleOk = () => {
	emits("claims", claimsToBeAdded.value)

	claimsToBeAdded.value = [];

	visible.value = false;
};

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
					tab="Add Manually"
				>
					<AddClaimsManual @claim="receiveClaim" />
				</a-tab-pane>
				<a-tab-pane
					key="2"
					tab="Extract with AI"
					force-render
				>
					<div>
						<AddClaimsAiExtraction 
						:source="source" 
						@claims="receiveClaims" />
					</div>
				</a-tab-pane>
				<a-tab-pane
					key="3"
					tab="Add JSON Object"
					force-render
				>
					<div>
						<AddClaimsFromJson @claims="receiveClaims" />
					</div>
				</a-tab-pane>
			</a-tabs>

			<div class="claims-to-be-added-list">
				<h3>Claims to be added</h3>
				<a-list
					item-layout="horizontal"
					:data-source="claimsToBeAdded"
				>
					<template #renderItem="{ item: claim }">
						<a-list-item>
							<template #actions>
								<!-- <a key="edit">Edit</a> -->
								<a
									key="delete"
									@click="removeClaim(claim.id)"
									>Remove</a
								>
							</template>
							<a-list-item-meta>
								<template #title>
									<div>
										<p class="claim-description">
											{{ getClaimDescriptionLine(claim) }}
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
