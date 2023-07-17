<script setup lang="ts">
import type ClaimCheck from "@/model/ClaimCheck";
import ClaimCheckReaction from "@/model/ClaimCheckReaction";
import { ref, onMounted } from "vue";
import { LikeFilled, LikeOutlined } from "@ant-design/icons-vue";
import { useClaimCheckReactionsStore } from "@/stores/claimCheckReactions";
import { generateGuid } from "@/utils/guid";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";
import { DeleteFilled } from "@ant-design/icons-vue";

interface Props {
	claimCheck: ClaimCheck;
	depth: number;
}

const { claimCheck, depth } = defineProps<Props>();

const emits = defineEmits(["removeClaimCheck"]);

const claimCheckReactionsStore = useClaimCheckReactionsStore();
const { userHasLikedClaimCheck, computeClaimCheckReactionSum } =
	claimCheckReactionsStore;

const claimCheckRating = ref(0);

const initLoading = ref(true);
const loadingClaimChecks = ref(false);

const fetchClaimCheckReactions = async () => {
	loadingClaimChecks.value = true;

	try {
		const claimCheckReactionssFound =
			await claimCheckReactionsStore.getClaimCheckReactionsAsync(claimCheck.id);

		claimCheck.claimCheckReactions = claimCheckReactionssFound;
	} catch (error) {
		console.error(error);
	} finally {
		loadingClaimChecks.value = false;
	}
};

const updateReactionsNumber = () => {
	claimCheckRating.value = computeClaimCheckReactionSum(
		claimCheck.claimCheckReactions
	);
};

onMounted(async () => {
	if (
		!initLoading &&
		(!claimCheck.claimCheckReactions ||
			claimCheck.claimCheckReactions.length === 0)
	) {
		await fetchClaimCheckReactions();
	}

	updateReactionsNumber();

	initLoading.value = false;
});

const addClaimCheckReaction = async (reaction: number) => {
	const claimCheckReaction = new ClaimCheckReaction();
	claimCheckReaction.id = generateGuid();
	claimCheckReaction.reaction = reaction;

	const createClaimCheckResult =
		await claimCheckReactionsStore.addClaimCheckReactionAsync(
			claimCheckReaction,
			claimCheck
		);

	if (!createClaimCheckResult) {
		NotificationError(
			"Failed to register reaction",
			`Reaction couldn't be registered: Internal error.`
		);
		return;
	}

	updateReactionsNumber();

	NotificationSuccess(
		"Reaction registered!",
		`Reaction registered successfully`
	);
};

const removeClaim = () => {
	emits("removeClaimCheck", claimCheck.id);
};
</script>

<template>
	<a-comment>
		<template #actions>
			<span key="comment-basic-like">
				<a-tooltip title="Like">
					<template
						v-if="userHasLikedClaimCheck(claimCheck.claimCheckReactions)"
					>
						<LikeFilled @click="addClaimCheckReaction(0)" />
					</template>
					<template v-else>
						<LikeOutlined @click="addClaimCheckReaction(1)" />
					</template>
				</a-tooltip>
				<span style="padding-left: 8px; cursor: auto">
					{{ claimCheckRating }}
				</span>
			</span>
			<!-- <span key="comment-nested-reply-to">Reply to</span> -->
			<span key="claimcheck-author"
				><a>Written by {{ claimCheck.userId }}</a></span
			>
			<a @click="removeClaim"><DeleteFilled /></a>
		</template>
		<!-- <template #avatar>
			<a-avatar
				src="https://joeschmoe.io/api/v1/random"
				alt="Han Solo"
			/>
		</template> -->
		<template #content>
			<div class="claimcheck-comment">
				<a-row class="claimcheck-label-row">
					<p class="claimcheck-label">{{ claimCheck.label }}</p>
				<a-row>
				</a-row>
					<p class="claimcheck-text">
						{{ claimCheck.claimCheckText }}
					</p>
				</a-row>
			</div>
		</template>

		<!-- TODO Add nested comments -->
	</a-comment>
</template>

<style scoped>
.claimcheck-label-row {
	display: flex;
}

.claimcheck-label {
	font-size: 2rem;
	font-weight: 600;
}

.claimcheck-text {
	font-size: medium;
	padding: 0 1vw;
}
</style>
