import { ref } from "vue";
import { defineStore } from "pinia";
import requestHandler from "@/utils/axioshandler";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";
import { useUserStore } from "@/stores/users";
import ClaimCheckResult from "@/model/ClaimCheckResult";

const { VITE_API_BASE_URL } = import.meta.env;

export const useFactChecksStore = defineStore(
	Keys.FACTCHECKS,
	() => {
		const userStore = useUserStore();
		const { userHasRole, Roles } = userStore;

		const errorMessages = ref<string[]>([]);
		const loading = ref(false);

		async function factCheckClaimsAsync(
			claimIds: string[]
		): Promise<ClaimCheckResult[]> {
			errorMessages.value = [];

			if (!userHasRole(Roles.ADDCLAIMCHECKSWITHAI)) {
				errorMessages.value.push(ErrorMessages.USER_ACCESS_ERROR);
				return [];
			}

			let claimCheckResults: ClaimCheckResult[] = [];

			try {
				loading.value = true;

				const backendResponse = await requestHandler<ClaimCheckResult[]>(
					{
						method: "post",
						url: `${VITE_API_BASE_URL}/api/factchecker`,
						data: claimIds
					},
					ErrorMessages.FACT_CHECK_ERROR
				);

				if (backendResponse.messages) {
					errorMessages.value = errorMessages.value.concat(
						backendResponse.messages
					);
				}

				if (backendResponse.data) {
					claimCheckResults = backendResponse.data || [];
				}
			} catch (error) {
				// Any unexpected error
				errorMessages.value.push(
					`Unexpected error: ${
						error instanceof Error ? error.message : String(error)
					}`
				);
			} finally {
				loading.value = false;
			}

			return claimCheckResults;
		}

		return {
			errorMessages,
			loading,
			factCheckClaimsAsync,
		};
	}
);
