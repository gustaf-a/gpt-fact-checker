<script setup lang="ts">
import Container from "@/components/Container.vue";
import useColorScheme from '@/stores/colorScheme'

//Ensure CSS Variables are created
useColorScheme()

</script>

<template>
	<a-layout-header class="hero-banner">
		<Container>
			<div class="hero-container">
				<h1 class="hero-title">Welcome to Fact Friend</h1>
				<div class="hero-subtext-container">
					<text class="hero-subtext">Clearly presented claims</text>
					<text class="hero-subtext">Easy fact checking</text>
					<text class="hero-subtext">AI assisted and human powered</text>
				</div>
			</div>
		</Container>
	</a-layout-header>
</template>

<style scoped>
.hero-banner {
	padding-top: 10vh;
	background-color: var(--color-background);
	height: 50vh;
}

.hero-container {
	padding: 0;
	display: flex;
	flex-direction: column;
	align-items: start;
	justify-content: center;
}

.hero-title {
	color: var(--color-text);
	opacity: 1;
	font-size: 4em;
	font-weight: bold;
	margin-bottom: 0.1em; /* Reduce the bottom margin */
}

.hero-subtext-container{
	padding: 0;
	margin-top: 5vh;
	display: flex;
	flex-direction: row;
	align-items: start;
	justify-content: center;
}

.hero-subtext {
	color: var(--color-text);
	margin: 0;
    margin-left: 6vw;
    margin-top: 0.05em; /* Decreased top margin */
    margin-bottom: 0.05em; /* Decreased bottom margin */
    font-size: 1.5em;
    opacity: 0.7;
}

</style>
