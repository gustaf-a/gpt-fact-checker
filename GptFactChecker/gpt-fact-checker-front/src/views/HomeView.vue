<script setup lang="ts">
import HomePage from "@/components/Content/HomePage/HomePage.vue";
import HeroBanner from "@/components/Content/HomePage/HeroBanner.vue";
import ProcessBanner from "@/components/Content/HomePage/ProcessOverViewBanner.vue";
</script>

<template>
	<main>
		<div class="content-container">
			<HeroBanner />
			<ProcessBanner />
			<HomePage />
		</div>
	</main>
</template>
