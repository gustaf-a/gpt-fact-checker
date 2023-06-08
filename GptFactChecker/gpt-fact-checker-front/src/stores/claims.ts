import { ref } from "vue";
import { defineStore } from "pinia";
import axios from "axios";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";
import Claim from "@/model/Claim";
import { useUserStore } from "@/stores/users";

const { VITE_API_BASE_URL } = import.meta.env;

export const useClaimsStore = defineStore(Keys.CLAIMS, () => {
	const userStore = useUserStore();
	const { userHasRole, Roles } = userStore;

	const errorMessage = ref("");
	const loadingClaims = ref(false);

	//TODO add local claims?

	async function getClaimsAsync(sourceId: string): Promise<Claim[]> {
		loadingClaims.value = true;

		try {
			const response = await axios.get(
				`${VITE_API_BASE_URL}/api/claims/source/id?sourceId=${sourceId}`
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
		if (!userHasRole(Roles.ADDCLAIMS)) return false;
		
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

			const response = await axios.post(`${VITE_API_BASE_URL}/api/claims/source/id?sourceId=${sourceId}`, claimsToAdd);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.CREATE_RESOURCE_ERROR;
				console.log(ErrorMessages.CREATE_RESOURCE_ERROR, claimsToAdd);
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

	async function getAllClaimsAsync(): Promise<Claim[]> {
		loadingClaims.value = true;
	
		try {
			const response = await axios.get(`${VITE_API_BASE_URL}/api/claims`);
	
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
	
	async function deleteClaimsAsync(claimId: string): Promise<boolean> {
		if (!userHasRole(Roles.DELETECLAIMS)) return false;
		
		try {
			const response = await axios.delete(`${VITE_API_BASE_URL}/api/claims/id?claimId=${claimId}`);
	
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

	return {
		errorMessage,
		loadingClaims,
		getClaimsAsync,
		getAllClaimsAsync,
		addClaimsAsync,
		deleteClaimsAsync
	};
});
