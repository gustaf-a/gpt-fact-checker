import { ref } from "vue";
import { defineStore } from "pinia";
import axios from "axios";
import Claim from "@/model/Claim";
import ClaimsToCreateDto from "@/model/ClaimsToCreateDto"
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";

const BaseUrl = "http://localhost:5067";

export const useClaimsStore = defineStore(Keys.CLAIMS, () => {
	const errorMessage = ref("");
	const loadingClaims = ref(false);

	async function getClaimsAsync(sourceId: string): Promise<Claim[]> {
		loadingClaims.value = true;

		try {
			const response = await axios.get(
				`${BaseUrl}/api/claims/source/id?sourceId=${sourceId}`
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
			loadingClaims.value = false;
		}

		return [];
	}

	async function addClaimsAsync(claimsToAdd: Claim[], sourceId: string) {
		if (!claimsToAdd || claimsToAdd.length == 0) {
			console.log("No claims to add found.");
			return false;
		}

		if(!sourceId || sourceId === ""){
			console.log("Failed to add claims: Invalid source ID.")
			return;
		}

		try {
			loadingClaims.value = true;

			console.log(`Sending to backend sourceId: ${sourceId}`);
			const response = await axios.post(`${BaseUrl}/api/claims/source/id?sourceId=${sourceId}`, claimsToAdd);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.CREATE_RESOURCE_ERROR;
				console.log(ErrorMessages.CREATE_RESOURCE_ERROR);
				console.log(claimsToAdd);
				return false;
			}

			return true;
		} catch (error) {
			errorMessage.value = `${ErrorMessages.CREATE_RESOURCE_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
			console.log(error, claimsToAdd);
			return false;
		} finally {
			loadingClaims.value = false;
		}
	}

	return {
		errorMessage,
		loadingClaims,
		getClaimsAsync,
		addClaimsAsync
	};
});
