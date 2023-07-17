import { ref } from "vue";
import { defineStore } from "pinia";
import requestHandler from "@/utils/axioshandler";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";
import { useUserStore } from "@/stores/users";
import Claim from "@/model/Claim";
import FactExtractionResponse from "@/model/FactExtractionResponse";

const { VITE_API_BASE_URL } = import.meta.env;

export const useFactExtractionsStore = defineStore(
	Keys.FACTEXTRACTIONS,
	() => {
		const userStore = useUserStore();
		const { userHasRole, Roles } = userStore;

		const errorMessages = ref<string[]>([]);
		const loadingExtractedClaims = ref(false);

		async function extractClaimsFromSourceWithAIAsync(
			sourceId: string
		): Promise<Claim[]> {
			errorMessages.value = [];

			if (!userHasRole(Roles.EXTRACTCLAIMSFROMSOURCEWITHAI)) {
				errorMessages.value.push(ErrorMessages.USER_ACCESS_ERROR);
				return [];
			}

			let claimsExtracted: Claim[] = [];

			try {
				loadingExtractedClaims.value = true;

				const backendResponse = await requestHandler<FactExtractionResponse>(
					{
						method: "get",
						url: `${VITE_API_BASE_URL}/api/factextractor/source/id?id=${sourceId}`,
					},
					ErrorMessages.CLAIM_EXTRACTION_ERROR
				);

				if (backendResponse.messages) {
					errorMessages.value = errorMessages.value.concat(
						backendResponse.messages
					);
				}

				if (backendResponse.data) {
					claimsExtracted = backendResponse.data.extractedClaims || [];
				}
			} catch (error) {
				// Any unexpected error
				errorMessages.value.push(
					`Unexpected error: ${
						error instanceof Error ? error.message : String(error)
					}`
				);
			} finally {
				loadingExtractedClaims.value = false;
			}

			return claimsExtracted;
		}

		return {
			errorMessages,
			loadingExtractedClaims,
			extractClaimsFromSourceWithAIAsync,
		};
	}
);
