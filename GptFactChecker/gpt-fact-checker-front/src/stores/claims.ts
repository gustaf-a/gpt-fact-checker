import { ref } from "vue";
import { defineStore } from "pinia";
import axios from "axios";
import Claim from "@/model/Claim";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";

const BaseUrl = "http://localhost:5067";

export const useClaimsStore = defineStore(Keys.CLAIMS, () => {
	const errorMessage = ref("");
	const loadingClaims = ref(false);

	async function getClaimsAsync(sourceId: string): Promise<Claim[]> {
		loadingClaims.value = true;

		try {
			const response = await axios.get(`${BaseUrl}/api/facts/source/id?sourceId=${sourceId}`);

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

	return {
		errorMessage,
		loadingClaims,
		getClaimsAsync
	};
});
