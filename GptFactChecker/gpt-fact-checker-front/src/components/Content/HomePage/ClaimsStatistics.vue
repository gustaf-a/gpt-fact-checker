<script setup lang="ts">
import SourceObject from "@/model/Source";

interface Props {
	source: SourceObject;
}

const props = defineProps<Props>();

//Get claims from store

</script>

<template>
    <p>5 Inaccurate claims</p>
    <p>3 Exaggerated and 1 misleading claim</p>
    <p>10 claims in need of fact checking</p>
</template>
