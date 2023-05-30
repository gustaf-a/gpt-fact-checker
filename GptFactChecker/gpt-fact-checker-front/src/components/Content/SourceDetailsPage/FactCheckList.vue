<script setup lang="ts">
import FactCheckComment from './FactCheckComment.vue';
import ClaimCheck from '@/model/ClaimCheck';

interface Props {
	claimChecks: ClaimCheck[];
}

const { claimChecks } = defineProps<Props>()

</script>

<template>
    <div v-for="claimCheck in claimChecks" class="claim-check-thread">
        <FactCheckComment :claimCheck="claimCheck" :depth="0"/>
    </div>
    <div class="add-claim-check">
        //TODO Modal with button to add claim check
    </div>
  </template>