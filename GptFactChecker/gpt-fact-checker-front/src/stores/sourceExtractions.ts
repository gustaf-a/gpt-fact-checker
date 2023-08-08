import { ref } from "vue";
import { defineStore } from "pinia";
import requestHandler from "@/utils/axioshandler";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";
import { useUserStore } from "@/stores/users";
import Source from "@/model/Source";
import SourceExtractionResponse from "@/model/SourceExtractionResponse";

const { VITE_API_BASE_URL } = import.meta.env;

export const useSourceExtractionsStore = defineStore(
	Keys.SOURCEEXTRACTIONS,
	() => {
		const userStore = useUserStore();
		const { userHasRole, Roles } = userStore;

		const errorMessages = ref<string[]>([]);
		const loadingExtractedSource = ref(false);
		const loadingSourceMetaData = ref(false);

		async function extractSourceAsync(
			sourceId: string
		): Promise<Source | undefined> {
			errorMessages.value = [];

			if (!userHasRole(Roles.EXTRACTSOURCES)) {
				errorMessages.value.push(ErrorMessages.USER_ACCESS_ERROR);
				return undefined;
			}

			let sourceExctracted: Source | undefined = undefined;

			try {
				loadingExtractedSource.value = true;

				const backendResponse = await requestHandler<SourceExtractionResponse>(
					{
						method: "get",
						url: `${VITE_API_BASE_URL}/api/sourceextractor/id?id=${sourceId}`,
					},
					ErrorMessages.SOURCE_EXTRACTION_ERROR
				);

				if (backendResponse.messages) {
					errorMessages.value = errorMessages.value.concat(
						backendResponse.messages
					);
				}

				if (backendResponse.data) {
					sourceExctracted = backendResponse.data.collectedSource;
				}

				return sourceExctracted;
			} catch (error) {
				errorMessages.value.push(
					`Unexpected error: ${
						error instanceof Error ? error.message : String(error)
					}`
				);
			} finally {
				loadingExtractedSource.value = false;
			}
		}

		async function extractSourceUrlMetaDataAsync(
			sourceUrl: string
		): Promise<Source | undefined> {
			errorMessages.value = [];

			if (!userHasRole(Roles.ADDSOURCES)) {
				errorMessages.value.push(ErrorMessages.USER_ACCESS_ERROR);
				return undefined;
			}

			let sourceWithMetaData: Source | undefined = undefined;

			try {
				loadingSourceMetaData.value = true;

				const backendResponse = await requestHandler<SourceExtractionResponse>(
					{
						method: "get",
						url: `${VITE_API_BASE_URL}/api/sourceextractor/sourceUrl?sourceUrl=${sourceUrl}`,
					},
					ErrorMessages.SOURCE_EXTRACTION_ERROR
				);

				if (backendResponse.messages) {
					errorMessages.value = errorMessages.value.concat(
						backendResponse.messages
					);
				}

				if (backendResponse.data) {
					sourceWithMetaData = backendResponse.data.collectedSource;
				}

				return sourceWithMetaData;
			} catch (error) {
				errorMessages.value.push(
					`Unexpected error: ${
						error instanceof Error ? error.message : String(error)
					}`
				);
			} finally {
				loadingSourceMetaData.value = false;
			}
		}

		return {
			errorMessages,
			loadingExtractedSource,
			loadingSourceMetaData,
			extractSourceAsync,
			extractSourceUrlMetaDataAsync,
		};
	}
);
