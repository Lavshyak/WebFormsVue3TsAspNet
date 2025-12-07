import {type JsonFormDto, postFormsSearchClient, type SearchingConfigurationDto} from "@/gen";
import {computed, readonly, ref} from "vue";

export function useFormsSearching() {
    const currentPageForms = ref<JsonFormDto[]>([]);
    const currentPageNumber = ref(1);

    const isLoading = ref(false);
    const isNextPageEmpty = ref(false);

    const currentSearchingOptions: {
        maxRowsOnPage?: number,
        formType?: string,
    } = {}

    function fetch(options: FormsSearchingOptions) {
        const data = {
            skip: options.pageNumber && options.maxRowsOnPage ? (options.pageNumber-1) * options.maxRowsOnPage : null,
            take: options.maxRowsOnPage ? options.maxRowsOnPage : null,
            pathsValues: (options.formType === null || options.formType?.trim() === '') ? null : [{
                path: ["formType"],
                valueSearchingConfigurationDto: {
                    exactStringValue: options.formType,
                }
            }]
        }

        return postFormsSearchClient(data);
    }

    function newSearch(options: FormsSearchingOptions) {
        if (isLoading.value) {
            throw new Error();
        }
        isLoading.value = true;

        currentSearchingOptions.formType = options.formType;
        currentSearchingOptions.maxRowsOnPage = options.maxRowsOnPage;
        currentPageNumber.value = 1;

        const data: FormsSearchingOptions = {
            formType: options.formType,
            maxRowsOnPage: options.maxRowsOnPage,
            pageNumber: 1
        };

        fetch(data).then((data) => {
            if (data.status !== 200) {
                throw new Error();
            }

            if (data.data.length == 0 || (options.maxRowsOnPage && data.data.length < options.maxRowsOnPage)) {
                isNextPageEmpty.value = true;
            } else {
                isNextPageEmpty.value = false;
            }

            currentPageForms.value = data.data;
        }).finally(() => isLoading.value = false);
    }

    function tryMoveToPage(pageNumber: number) {
        if (isLoading.value) {
            throw new Error();
        }
        isLoading.value = true;

        const data: FormsSearchingOptions = {
            formType: currentSearchingOptions.formType,
            maxRowsOnPage: currentSearchingOptions.maxRowsOnPage,
            pageNumber: pageNumber
        };

        fetch(data).then((data) => {
            if (data.status !== 200) {
                throw new Error();
            }

            if (data.data.length == 0) {
                isNextPageEmpty.value = true;
                if(pageNumber > currentPageNumber.value){
                    return;
                }
            }
            else if(currentSearchingOptions.maxRowsOnPage && data.data.length < currentSearchingOptions.maxRowsOnPage){
                isNextPageEmpty.value = true;
            }
            else {
                isNextPageEmpty.value = false;
            }

            currentPageForms.value = data.data;
            currentPageNumber.value = pageNumber;
        }).finally(() => isLoading.value = false);
    }

    return {
        currentPageForms: readonly(currentPageForms),
        currentPageNumber: readonly(currentPageNumber),
        isFetching: readonly(isLoading),
        nextPageIsEmpty: readonly(isNextPageEmpty),
        newSearch,
        tryMoveToPage
    }
}

export type FormsSearchingOptions = {
    formType?: string,
    pageNumber?: number,
    maxRowsOnPage?: number,
}