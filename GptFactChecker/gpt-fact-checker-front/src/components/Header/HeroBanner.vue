<script setup lang="ts">
import Container from "../Container.vue";
</script>

<template>
	<a-layout-header class="hero-banner">
		<Container>
			<div class="hero-container">
				<h1 class="hero-title">FactCheckMe</h1>
				<p class="hero-subtext">
					Clearly presented claims - easy to check
				</p>
			</div>
		</Container>
	</a-layout-header>
</template>
<style scoped>
.hero-banner {
	background-color: #9f9f9f;
	height: 15vh;
}

.hero-container {
	padding: 2.5vh 0;
	display: flex;
	color: #ffffff;
	align-self: center;
	justify-content: center;
}

.hero-title {
	color: #ffffff;
	opacity: 0.7;
	font-size: 3em;
	font-weight: bold;
	margin-bottom: 0.1em; /* Reduce the bottom margin */
}

.hero-subtext {
	margin-left: 2vw;
	font-size: 1.2em;
	opacity: 0.7;
	margin-top: 0.1em; /* Add a small top margin */
}
</style>
