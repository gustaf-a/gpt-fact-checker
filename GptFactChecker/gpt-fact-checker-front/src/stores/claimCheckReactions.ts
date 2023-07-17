import { ref } from "vue";
import { defineStore, storeToRefs } from "pinia";
import axios from "axios";
import requestHandler from "@/utils/axioshandler";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";
import ClaimCheckReaction from "@/model/ClaimCheckReaction";
import { useUserStore } from "@/stores/users";
import type ClaimCheck from "@/model/ClaimCheck";

const { VITE_API_BASE_URL } = import.meta.env;

export const useClaimCheckReactionsStore = defineStore(
	Keys.CLAIMCHECKREACTIONS,
	() => {
		const userStore = useUserStore();
		const { userHasRole, Roles } = userStore;
		const { user } = storeToRefs(userStore);

		const errorMessage = ref("");
		const loadingClaimCheckReactions = ref(false);

		async function getClaimCheckReactionsAsync(
			claimCheckId: string
		): Promise<ClaimCheckReaction[]> {
			loadingClaimCheckReactions.value = true;

			try {
				const response = await axios.get(
					`${VITE_API_BASE_URL}/api/claimcheckreactions/claimcheck/id?claimCheckId=${claimCheckId}`
				);

				if (response.status !== 200) {
					errorMessage.value = ErrorMessages.DATA_FETCH_ERROR;
					return [];
				}

				return response.data;
			} catch (error) {
				errorMessage.value = `${ErrorMessages.DATA_FETCH_ERROR}: ${
					error instanceof Error ? error.message : String(error)
				}`;
			} finally {
				loadingClaimCheckReactions.value = false;
			}

			return [];
		}

		async function addClaimCheckReactionAsync(
			claimCheckReaction: ClaimCheckReaction,
			claimCheck: ClaimCheck
		) {
			if (!userHasRole(Roles.ADDCLAIMCHECKREACTIONS)) return;

			if (!claimCheck) {
				console.log("No ClaimCheck object found to add reaction to.");
				return false;
			}

			if (!claimCheckReaction) {
				console.log("No claim check reaction to add found.");
				return false;
			}

			if (!claimCheck.id || claimCheck.id === "") {
				console.log(
					"Failed to add claim check reactions: Invalid claim check ID."
				);
				return;
			}

			const userId = user.value?.id;
			if (!userId) return;

			claimCheckReaction.userId = userId;

			try {
				loadingClaimCheckReactions.value = true;

				const backendResponse = await requestHandler<boolean>(
					{
						method: "post",
						url: `${VITE_API_BASE_URL}/api/claimcheckreactions/claimcheck/id?claimCheckId=${claimCheck.id}`,
						data: claimCheckReaction,
					},
					ErrorMessages.CREATE_RESOURCE_ERROR
				);

				if (backendResponse.messages) {
					backendResponse.messages.forEach((msg) => {
						console.log(msg);
					});
				}

				if (!backendResponse.isSuccess) {
					console.log("Failed to add claim check reaction.");
					return false;
				}

				addReactionToClaimCheck(claimCheck, claimCheckReaction);

				return true;
			} catch (error) {
				// Any unexpected error
				console.log(
					`Unexpected error: ${
						error instanceof Error ? error.message : String(error)
					}`
				);
			} finally {
				loadingClaimCheckReactions.value = false;
			}
		}

		function addReactionToClaimCheck(
			claimCheck: ClaimCheck,
			reaction: ClaimCheckReaction
		): void {
			if (claimCheck.claimCheckReactions === undefined) {
				claimCheck.claimCheckReactions = [];
			}

			claimCheck.claimCheckReactions = claimCheck.claimCheckReactions.filter(
				(reactionItem) => reactionItem.userId !== reaction.userId
			);

			claimCheck.claimCheckReactions.push(reaction);
		}

		// async function getAllClaimCheckReactionsAsync(): Promise<
		// 	ClaimCheckReaction[]
		// > {
		// 	loadingClaimCheckReactions.value = true;

		// 	try {
		// 		const response = await axios.get(
		// 			`${VITE_API_BASE_URL}/api/claimcheckreactions`
		// 		);

		// 		if (response.status !== 200) {
		// 			errorMessage.value = ErrorMessages.DATA_FETCH_ERROR;
		// 			return [];
		// 		}

		// 		return response.data;
		// 	} catch (error) {
		// 		errorMessage.value = `${ErrorMessages.DATA_FETCH_ERROR}: ${
		// 			error instanceof Error ? error.message : String(error)
		// 		}`;
		// 	} finally {
		// 		loadingClaimCheckReactions.value = false;
		// 	}

		// 	return [];
		// }

		async function deleteClaimCheckReactionsAsync(
			claimCheckReactionId: string
		): Promise<boolean> {
			if (!userHasRole(Roles.DELETECLAIMCHECKREACTIONS)) return false;

			try {
				const response = await axios.delete(
					`${VITE_API_BASE_URL}/api/claimcheckreactions?id=${claimCheckReactionId}`
				);

				if (response.status !== 200) {
					errorMessage.value = ErrorMessages.DELETE_RESOURCE_ERROR;
					return false;
				}

				return true;
			} catch (error) {
				errorMessage.value = `${ErrorMessages.DELETE_RESOURCE_ERROR}: ${
					error instanceof Error ? error.message : String(error)
				}`;
				return false;
			}
		}

		const userHasLikedClaimCheck = (
			claimCheckReactions: ClaimCheckReaction[] | undefined
		): boolean => {
			if (!claimCheckReactions) return false;

			const userId = user.value?.id;

			if (!userId) return false;

			const usersReaction = claimCheckReactions.find(
				(reaction) => reaction.userId === userId
			);

			if (!usersReaction) return false;

			return usersReaction.reaction > 0;
		};

		const computeClaimCheckReactionSum = (
			claimCheckReactions: ClaimCheckReaction[] | undefined
		): number => {
			if (!claimCheckReactions || claimCheckReactions.length === 0) {
				return 0;
			}

			let sum = 0;

			claimCheckReactions.forEach((claimCheckReaction) => {
				sum += claimCheckReaction.reaction;
			});

			return sum;
		};

		return {
			errorMessage,
			loadingClaimCheckReactions,
			getClaimCheckReactionsAsync,
			// getAllClaimCheckReactionsAsync,
			addClaimCheckReactionAsync,
			deleteClaimCheckReactionsAsync,
			userHasLikedClaimCheck,
			computeClaimCheckReactionSum,
		};
	}
);
