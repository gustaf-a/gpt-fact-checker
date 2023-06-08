import { ref } from "vue";
import { defineStore, storeToRefs } from "pinia";
import axios from "axios";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";
import ClaimCheck from "@/model/ClaimCheck";
import { useUserStore } from "@/stores/users";

const { VITE_API_BASE_URL } = import.meta.env;

export const useClaimCheckStore = defineStore(Keys.CLAIMCHECKS, () => {
	const userStore = useUserStore();
	const { user } = storeToRefs(userStore);
	const { userHasRole, Roles } = userStore;

	const errorMessage = ref("");
	const loadingClaimChecks = ref(false);

	async function getClaimChecksAsync(claimId: string): Promise<ClaimCheck[]> {
		loadingClaimChecks.value = true;

		if (!claimId || claimId === "") {
			console.log("Failed to get claimChecks: Invalid claim ID.");
			return [];
		}

		try {
			const response = await axios.get(
				`${VITE_API_BASE_URL}/api/claimchecks/claim/id?claimId=${claimId}`
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
			loadingClaimChecks.value = false;
		}

		return [];
	}

	async function addClaimCheckAsync(
		claimCheckToAdd: ClaimCheck,
		claimId: string | undefined
	) {
		return await addClaimChecksAsync([claimCheckToAdd], claimId);
	}

	async function addClaimChecksAsync(
		claimChecksToAdd: ClaimCheck[],
		claimId: string | undefined
	) {
		if (!userHasRole(Roles.ADDCLAIMCHECKS)) return;

		if (!claimChecksToAdd || claimChecksToAdd.length == 0) {
			console.log("No claim checks to add found.");
			return false;
		}

		if (!claimId || claimId === "") {
			console.log("Failed to add claim checks: Invalid claim ID.");
			return;
		}

		if (isInvalidClaimChecks(claimChecksToAdd)) {
			console.log(
				"Failed to add claim checks: ClaimChecks invalid.",
				claimChecksToAdd
			);
			return;
		}

		try {
			loadingClaimChecks.value = true;

			const response = await axios.post(
				`${VITE_API_BASE_URL}/api/claimchecks/claim/id?claimId=${claimId}`,
				claimChecksToAdd
			);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.CREATE_RESOURCE_ERROR;
				console.log(ErrorMessages.CREATE_RESOURCE_ERROR, claimChecksToAdd);
				return false;
			}

			return true;
		} catch (error) {
			errorMessage.value = `${ErrorMessages.CREATE_RESOURCE_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
			console.log(error, claimChecksToAdd);
			return false;
		} finally {
			loadingClaimChecks.value = false;
		}
	}

	function isInvalidClaimChecks(claimChecks: ClaimCheck[]): boolean {
		for (const claimCheck of claimChecks) {
			if (isInvalidClaimCheck(claimCheck)) return true;
		}

		return false;
	}

	function isInvalidClaimCheck(claimCheck: ClaimCheck): boolean {
		if (
			!claimCheck.id ||
			claimCheck.id.trim() === "" ||
			!claimCheck.userId ||
			claimCheck.userId.trim() === "" ||
			!claimCheck.label ||
			claimCheck.label.trim() === "" ||
			!claimCheck.claimCheckText ||
			claimCheck.claimCheckText.trim() === ""
		) {
			return true;
		}

		return false;
	}

	async function getAllClaimChecksAsync(): Promise<ClaimCheck[]> {
		loadingClaimChecks.value = true;

		try {
			const response = await axios.get(`${VITE_API_BASE_URL}/api/claimchecks`);

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
			loadingClaimChecks.value = false;
		}

		return [];
	}

	async function deleteClaimChecksAsync(
		claimCheckId: string
	): Promise<boolean> {
		if (!userHasRole(Roles.DELETECLAIMCHECKS)) return false;

		try {
			const response = await axios.delete(
				`${VITE_API_BASE_URL}/api/claimchecks/id?claimCheckId=${claimCheckId}`
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

	return {
		errorMessage,
		loadingClaimChecks,
		getClaimChecksAsync,
		getAllClaimChecksAsync,
		addClaimCheckAsync,
		addClaimChecksAsync,
		deleteClaimChecksAsync,
	};
});
