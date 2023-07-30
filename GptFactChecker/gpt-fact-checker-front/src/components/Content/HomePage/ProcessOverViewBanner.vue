<script setup lang="ts">
import Container from "@/components/Container.vue";
import useColorScheme from '@/stores/colorScheme'

//Ensure CSS Variables are created
useColorScheme()

</script>

<template>
	<a-layout-header class="banner">
		<Container>
			<div class="container">
				<h1 class="title">How it works</h1>
				<div class="subtext-container">
					<text class="subtext">Source of information</text>
					<text class="subtext">-></text>
					<text class="subtext">Listed claims</text>
					<text class="subtext">-></text>
					<text class="subtext">Fact checks</text>
				</div>
			</div>
		</Container>
	</a-layout-header>
</template>

<style scoped>
.banner {
	padding-top: 10vh;
	background-color: var(--color-backgroundSecond);
	height: 60vh;
}

.container {
	padding: 0;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
}

.title {
	color: var(--color-text);
	opacity: 1;
	font-size: 4em;
	font-weight: bold;
	margin-bottom: 0.1em; /* Reduce the bottom margin */
}

.subtext-container{
	margin-top: 5vh;
	padding: 0;
	display: flex;
	flex-direction: row;
	align-items: start;
	justify-content: center;
}

.subtext {
	color: var(--color-text);
	margin: 0;
    margin-left: 6vw;
    margin-top: 0.05em; /* Decreased top margin */
    margin-bottom: 0.05em; /* Decreased bottom margin */
    font-size: 1.5em;
    opacity: 0.7;
}

</style>
